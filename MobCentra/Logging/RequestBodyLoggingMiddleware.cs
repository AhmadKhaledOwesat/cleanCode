using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

namespace MobCentra.Logging;

public sealed class RequestBodyLoggingMiddleware : IMiddleware
{
    private static readonly HashSet<string> BodyContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/json",
        "application/*+json",
        "application/x-www-form-urlencoded",
        "text/plain",
        "text/json"
    };

    private readonly RequestLoggingOptions _options;

    public RequestBodyLoggingMiddleware(IOptions<RequestLoggingOptions> options)
    {
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (ShouldSkip(context.Request.Path))
        {
            await next(context);
            return;
        }

        var sw = Stopwatch.StartNew();

        string? requestBody = null;
        Dictionary<string, object?>? query = null;
        Dictionary<string, object?>? route = null;

        try
        {
            query = context.Request.Query.Count == 0
                ? null
                : context.Request.Query.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (object?)MaskIfNeeded(kvp.Key, kvp.Value.Count <= 1 ? kvp.Value.ToString() : kvp.Value.ToArray()),
                    StringComparer.OrdinalIgnoreCase);

            route = context.GetRouteData()?.Values?.Count > 0
                ? context.GetRouteData()!.Values.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (object?)MaskIfNeeded(kvp.Key, kvp.Value),
                    StringComparer.OrdinalIgnoreCase)
                : null;

            requestBody = await TryReadBodyAsync(context);

            await next(context);
        }
        catch
        {
            throw;
        }
        finally
        {
            sw.Stop();

            var status = context.Response.StatusCode;
            var evtLevel = status >= 500 ? LogEventLevel.Error :
                status >= 400 ? LogEventLevel.Warning :
                LogEventLevel.Information;

            var logger = Log.ForContext("RequestId", context.TraceIdentifier)
                .ForContext("Method", context.Request.Method)
                .ForContext("Path", context.Request.Path.Value ?? string.Empty)
                .ForContext("Url", context.Request.GetDisplayUrl())
                .ForContext("StatusCode", status)
                .ForContext("ElapsedMs", sw.ElapsedMilliseconds);

            if (query is not null) logger = logger.ForContext("Query", query, destructureObjects: true);
            if (route is not null) logger = logger.ForContext("Route", route, destructureObjects: true);
            if (!string.IsNullOrWhiteSpace(requestBody)) logger = logger.ForContext("Body", requestBody);

            logger.Write(evtLevel, "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs} ms");
        }
    }

    private bool ShouldSkip(PathString path)
    {
        if (_options.ExcludePaths is null || _options.ExcludePaths.Length == 0) return false;
        var p = path.Value ?? string.Empty;
        return _options.ExcludePaths.Any(x => !string.IsNullOrWhiteSpace(x) && p.StartsWith(x, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<string?> TryReadBodyAsync(HttpContext context)
    {
        var req = context.Request;
        if (req.Body is null) return null;

        if (req.ContentLength is 0) return null;
        if (!req.Headers.TryGetValue("Content-Type", out var ctValues)) return null;

        var contentType = (ctValues.ToString() ?? string.Empty).Split(';', 2)[0].Trim();
        if (!IsLoggableContentType(contentType)) return null;

        // Avoid dumping huge uploads (multipart files etc.)
        if (contentType.StartsWith("multipart/", StringComparison.OrdinalIgnoreCase))
            return "[multipart omitted]";

        req.EnableBuffering();
        req.Body.Position = 0;

        using var reader = new StreamReader(req.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
        var raw = await reader.ReadToEndAsync();
        req.Body.Position = 0;

        if (string.IsNullOrWhiteSpace(raw)) return null;

        var max = _options.MaxBodyChars <= 0 ? 20_000 : _options.MaxBodyChars;
        if (raw.Length > max) raw = raw[..max] + $"... [truncated {raw.Length - max} chars]";

        // Best-effort masking for common key/value bodies.
        if (contentType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
            return MaskFormUrlEncoded(raw);

        return MaskJsonLike(raw);
    }

    private bool IsLoggableContentType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType)) return false;
        if (contentType.StartsWith("multipart/", StringComparison.OrdinalIgnoreCase)) return true;
        return BodyContentTypes.Contains(contentType) || contentType.EndsWith("+json", StringComparison.OrdinalIgnoreCase);
    }

    private object? MaskIfNeeded(string key, object? value)
    {
        if (_options.MaskKeys?.Any(m => key.Contains(m, StringComparison.OrdinalIgnoreCase)) == true)
            return "[masked]";
        return value;
    }

    private string MaskFormUrlEncoded(string body)
    {
        // key=value&key2=value2...
        var parts = body.Split('&', StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < parts.Length; i++)
        {
            var eq = parts[i].IndexOf('=');
            if (eq <= 0) continue;
            var key = Uri.UnescapeDataString(parts[i][..eq]);
            if (_options.MaskKeys?.Any(m => key.Contains(m, StringComparison.OrdinalIgnoreCase)) == true)
                parts[i] = parts[i][..(eq + 1)] + "[masked]";
        }
        return string.Join('&', parts);
    }

    private string MaskJsonLike(string body)
    {
        // Very small best-effort mask without parsing:
        // replace "...\"password\"...: \"...\"" etc.
        if (_options.MaskKeys is null || _options.MaskKeys.Length == 0) return body;

        var masked = body;
        foreach (var k in _options.MaskKeys.Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            // naive patterns: "key":"value" OR "key" : "value" OR "key":"123"
            masked = System.Text.RegularExpressions.Regex.Replace(
                masked,
                $"(\"{System.Text.RegularExpressions.Regex.Escape(k)}\"\\s*:\\s*\")([^\"]*)(\")",
                "$1[masked]$3",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            masked = System.Text.RegularExpressions.Regex.Replace(
                masked,
                $"(\"{System.Text.RegularExpressions.Regex.Escape(k)}\"\\s*:\\s*)(\\d+|true|false|null)",
                "$1\"[masked]\"",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
        return masked;
    }
}

