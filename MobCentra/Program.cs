using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using MobCentra.Application.Bll;
using MobCentra.Application.DI;
using MobCentra.Application.Hubs;
using MobCentra.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;
using System.Net;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDcpMapper();
builder.Services.AddSignalR();
builder.Services.AddEfDbContext(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddServices();
builder.Services.AddOptions<EmailOptions>()
     .Bind(builder.Configuration.GetSection("Email"));
builder.Services.AddCors(op =>
{
    op.AddPolicy("AllowAllOrigins", policy =>
        policy.WithOrigins("https://mobcentra.com", "http://mobcentra.com", "http://localhost:4200", "https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = 50 * 1024 * 1024; // 30 MB
    o.MultipartBodyLengthLimit = 50 * 1024 * 1024;
    o.MemoryBufferThreshold = 1024 * 1024;
});
builder.Services.AddOpenApi();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is required"))),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;
    });
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = (int)HttpStatusCode.TooManyRequests;
    options.AddFixedWindowLimiter("api", config =>
    {
        config.PermitLimit = 20;
        config.Window = TimeSpan.FromMinutes(15);
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        config.QueueLimit = 0;
    });
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 30 MB
});
//#if !DEBUG
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(1999);
//});
//#endif

builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 50 * 1024 * 1024; // 10MB
    options.StreamBufferCapacity = 100;
});
var app = builder.Build();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        var error = contextFeature?.Error;
        var statusCode = error switch
        {
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            KeyNotFoundException or FileNotFoundException => HttpStatusCode.NotFound,
            ArgumentException or InvalidOperationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        var message = statusCode == HttpStatusCode.InternalServerError ? "An error occurred." : (error?.Message ?? "An error occurred.");
        await context.Response.WriteAsync(JsonConvert.SerializeObject(new DcpResponse<string>(default!, message, false), new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        }));
    });
});

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    await next();
});
app.UseRateLimiter();
if (!app.Environment.IsDevelopment())
    app.UseHsts();
app.UseRouting();
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<ScreenHub>("/screenHub");
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("api");
app.MapScalarApiReference();
app.MapOpenApi();
app.Run();


