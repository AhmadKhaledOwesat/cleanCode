using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using MobCentra.Application.Bll;
using MobCentra.Application.DI;
using MobCentra.Application.Hubs;
using MobCentra.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;
using System.Net;

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
    op.AddPolicy("AllowAllOrigins", builder =>
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = null;
});
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = false,
//        ValidateIssuerSigningKey = false,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Issuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
//    };
//});
//builder.Services.AddHostedService<MobCentra.Application.HostedService.BackgroundService>();
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
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = "application/json";
        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new DcpResponse<string>(contextFeature.Error.InnerException?.Message ?? contextFeature.Error.Message, contextFeature.Error.InnerException?.Message ?? contextFeature.Error.Message, false), new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            }));
    });
});

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<ScreenHub>("/screenHub");
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapScalarApiReference();
app.MapOpenApi();
app.Run();


