namespace MobCentra.Logging;

public sealed class RequestLoggingOptions
{
    public int MaxBodyChars { get; set; } = 20_000;

    public string[] ExcludePaths { get; set; } = [];

    public string[] MaskKeys { get; set; } =
    [
        "password",
        "pass",
        "pwd",
        "token",
        "access_token",
        "refresh_token",
        "authorization",
        "api_key",
        "apikey",
        "secret"
    ];
}

