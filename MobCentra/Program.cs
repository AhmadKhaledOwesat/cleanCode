using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using MobCentra.Application.Bll;
using MobCentra.Application.DI;
using MobCentra.Application.Hubs;
using MobCentra.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;
using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
#if !DEBUG
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(1999);
});
#endif
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

// SignalR Hub
#pragma warning disable CA1050 // Declare types in namespaces
public class ScreenHub : Hub
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Store last known screen size to send to newly connected clients
    private static int _lastScreenWidth = 0;
    private static int _lastScreenHeight = 0;
    private static readonly object _screenSizeLock = new();

    // Device registry - stores device information by connection ID
    public static readonly ConcurrentDictionary<string, DeviceInfo> _devices = new();
    private static readonly object _deviceLock = new();

    // Connection requests - stores pending connection requests
    private static readonly ConcurrentDictionary<string, string> _connectionRequests = new();

    public class DeviceInfo
    {
        public string DeviceId { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public string ConnectionId { get; set; } = "";
        public string CompanyId { get; set; } = "";
        public DateTime LastSeen { get; set; } = DateTime.Now;
        public bool IsSharing { get; set; } = false;
        public int? ScreenWidth { get; set; }
        public int? ScreenHeight { get; set; }
        public string? DeviceModel { get; set; }
        public string? Manufacturer { get; set; }
        public string? Brand { get; set; }
        public string? Product { get; set; }
        public string? Hardware { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "deviceId", DeviceId },
                { "deviceName", DeviceName },
                { "connectionId", ConnectionId },
                { "companyId", CompanyId },
                { "lastSeen", LastSeen.ToString("O") },
                { "isSharing", IsSharing },
                { "screenWidth", ScreenWidth ?? 0 },
                { "screenHeight", ScreenHeight ?? 0 },
                { "deviceModel", DeviceModel ?? "" },
                { "manufacturer", Manufacturer ?? "" },
                { "brand", Brand ?? "" },
                { "product", Product ?? "" },
                { "hardware", Hardware ?? "" }
            };
        }
    }

    // Called when a Flutter client sends screen data
    public async Task SendScreenData(object imageData, string timestamp)
    {
        try
        {
            Console.WriteLine($"SendScreenData called with imageData type: {imageData?.GetType()?.FullName ?? "null"}");

            // Log the actual data structure for debugging
            if (imageData != null)
            {
                try
                {
                    var jsonPreview = JsonSerializer.Serialize(imageData);
                    var previewLength = Math.Min(200, jsonPreview.Length);
                    Console.WriteLine($"Data preview (first {previewLength} chars): {jsonPreview.Substring(0, previewLength)}...");
                }
                catch (Exception logEx)
                {
                    Console.WriteLine($"Could not serialize data for preview: {logEx.Message}");
                }
            }

            byte[]? bytes = null;

            // Handle different input types that SignalR might send
            if (imageData == null)
            {
                Console.WriteLine("Warning: Received null image data");
                return;
            }

            // Try byte array first
            if (imageData is byte[] byteArray)
            {
                bytes = byteArray;
                Console.WriteLine("Data is byte[]");
            }
            // Try List<int> (most common from Flutter SignalR)
            else if (imageData is System.Collections.Generic.List<int> intList)
            {
                Console.WriteLine($"Data is List<int> with {intList.Count} elements");
                bytes = intList.Select(b => (byte)b).ToArray();
            }
            // Try List<object>
            else if (imageData is System.Collections.Generic.List<object> objList)
            {
                Console.WriteLine($"Data is List<object> with {objList.Count} elements");
                bytes = objList.Select(b => Convert.ToByte(b)).ToArray();
            }
            // Try Array
            else if (imageData is System.Array array)
            {
                Console.WriteLine($"Data is Array with {array.Length} elements, type: {array.GetType().GetElementType()?.Name ?? "unknown"}");
                var list = new List<byte>();
                foreach (var item in array)
                {
                    try
                    {
                        list.Add(Convert.ToByte(item));
                    }
                    catch (Exception convEx)
                    {
                        Console.WriteLine($"Failed to convert array item {item} (type: {item?.GetType()?.Name ?? "null"}): {convEx.Message}");
                        throw;
                    }
                }
                bytes = list.ToArray();
            }
            // Try JSON string (SignalR might serialize it)
            else if (imageData is string jsonString)
            {
                Console.WriteLine($"Data is string, attempting to parse as JSON array");
                try
                {
                    var intArray = JsonSerializer.Deserialize<int[]>(jsonString);
                    if (intArray != null)
                    {
                        bytes = intArray.Select(b => (byte)b).ToArray();
                    }
                    else
                    {
                        // Try as base64 string
                        try
                        {
                            bytes = Convert.FromBase64String(jsonString);
                            Console.WriteLine("Parsed as base64 string");
                        }
                        catch
                        {
                            throw new ArgumentException("String is not valid JSON array or base64");
                        }
                    }
                }
                catch (Exception jsonEx)
                {
                    Console.WriteLine($"Failed to parse JSON string: {jsonEx.Message}");
                    throw;
                }
            }
            // Try JsonElement (System.Text.Json)
            else if (imageData is JsonElement jsonElement)
            {
                Console.WriteLine($"Data is JsonElement, type: {jsonElement.ValueKind}");
                if (jsonElement.ValueKind == JsonValueKind.Array)
                {
                    var intListFromJson = new List<int>();
                    foreach (var element in jsonElement.EnumerateArray())
                    {
                        if (element.ValueKind == JsonValueKind.Number)
                        {
                            intListFromJson.Add(element.GetInt32());
                        }
                    }
                    bytes = intListFromJson.Select(b => (byte)b).ToArray();
                    Console.WriteLine($"Converted JsonElement array with {bytes.Length} bytes");
                }
                else
                {
                    throw new ArgumentException($"JsonElement is not an array, it's {jsonElement.ValueKind}");
                }
            }
            // Try to deserialize from JSON as last resort
            else
            {
                Console.WriteLine($"Unknown type, attempting JSON deserialization...");
                try
                {
                    var jsonStringForDeserialize = JsonSerializer.Serialize(imageData);
                    Console.WriteLine($"Serialized to JSON: {jsonStringForDeserialize.Substring(0, Math.Min(300, jsonStringForDeserialize.Length))}...");

                    // Try to deserialize as List<int>
                    var deserialized = JsonSerializer.Deserialize<List<int>>(jsonStringForDeserialize);
                    if (deserialized != null)
                    {
                        bytes = deserialized.Select(b => (byte)b).ToArray();
                        Console.WriteLine("Successfully deserialized as List<int> from JSON");
                    }
                    else
                    {
                        throw new ArgumentException($"Could not deserialize as List<int>");
                    }
                }
                catch (Exception jsonEx)
                {
                    Console.WriteLine($"JSON deserialization failed: {jsonEx.Message}");
                    throw new ArgumentException($"Unsupported imageData type: {imageData.GetType().FullName}. Serialization attempt: {jsonEx.Message}");
                }
            }

            if (bytes == null || bytes.Length == 0)
            {
                Console.WriteLine("Warning: Received empty image data after conversion");
                return;
            }

            // Validate that it's a valid image (check for common image file signatures)
            bool isValidImage = false;
            if (bytes.Length >= 4)
            {
                // PNG: 89 50 4E 47
                // JPEG: FF D8 FF
                // GIF: 47 49 46 38
                if ((bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47) || // PNG
                    (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF) || // JPEG
                    (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x38)) // GIF
                {
                    isValidImage = true;
                    Console.WriteLine($"Valid image signature detected. Type: {(bytes[0] == 0x89 ? "PNG" : bytes[0] == 0xFF ? "JPEG" : "GIF")}");
                }
                else
                {
                    Console.WriteLine($"Warning: Data doesn't have valid image signature. First bytes: {bytes[0]:X2} {bytes[1]:X2} {bytes[2]:X2} {bytes[3]:X2}");
                    // Continue anyway - might be valid image data with different encoding
                    isValidImage = true; // Assume valid for now
                }
            }
            else
            {
                Console.WriteLine("Warning: Data too short to be a valid image");
                return;
            }

            Console.WriteLine($"Received screen data: {bytes.Length} bytes at {timestamp}, valid: {isValidImage}");

            // Broadcast the screen data to all connected web clients
            try
            {
                await Clients.All.SendAsync("ReceiveScreenData", bytes, timestamp);
                Console.WriteLine($"Broadcasted {bytes.Length} bytes to clients");
            }
            catch (Exception broadcastEx)
            {
                Console.WriteLine($"ERROR broadcasting to clients: {broadcastEx.GetType().Name}: {broadcastEx.Message}");
                // Don't throw - just log the error and continue
                // This prevents the connection from closing due to broadcast errors
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in SendScreenData: {ex.GetType().Name}: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }

            // Don't re-throw the exception - this prevents the connection from closing
            // Just log the error and return silently
            // The client can continue sending frames even if one fails
            return;
        }
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Client connected: {Context.ConnectionId}");

        // If we have a stored screen size, send it to the newly connected client
        lock (_screenSizeLock)
        {
            if (_lastScreenWidth > 0 && _lastScreenHeight > 0)
            {
                Console.WriteLine($"Sending stored screen size to new client: {_lastScreenWidth}x{_lastScreenHeight}");
                // Send asynchronously without awaiting to avoid blocking
                _ = Clients.Client(Context.ConnectionId).SendAsync("ReceiveScreenSize", _lastScreenWidth, _lastScreenHeight);
            }
        }

        await base.OnConnectedAsync();
    }

    // Register a device (called by mobile app)
    public async Task RegisterDevice(string deviceId, string deviceName, string? companyId = null,
        string? deviceModel = null, string? manufacturer = null, string? brand = null,
        string? product = null, string? hardware = null)
    {
        try
        {
            var deviceInfo = new DeviceInfo
            {
                DeviceId = deviceId,
                DeviceName = deviceName,
                ConnectionId = Context.ConnectionId,
                CompanyId = companyId ?? "",
                LastSeen = DateTime.Now,
                DeviceModel = deviceModel,
                Manufacturer = manufacturer,
                Brand = brand,
                Product = product,
                Hardware = hardware
            };

            _devices.AddOrUpdate(Context.ConnectionId, deviceInfo, (key, oldValue) => deviceInfo);

            Console.WriteLine($"Device registered: {deviceName} ({deviceId}) - CompanyId: {companyId ?? "none"} - Model: {deviceModel ?? "unknown"} - Manufacturer: {manufacturer ?? "unknown"} - Connection: {Context.ConnectionId}");

            // Notify all viewers about the new device
            var deviceDict = deviceInfo.ToDictionary();
            await Clients.All.SendAsync("DeviceAdded", deviceDict);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in RegisterDevice: {ex.Message}");
            throw;
        }
    }

    // Update device status (sharing/not sharing)
    public async Task UpdateDeviceStatus(bool isSharing, int screenWidth = 0, int screenHeight = 0)
    {
        try
        {
            if (_devices.TryGetValue(Context.ConnectionId, out var deviceInfo))
            {
                deviceInfo.IsSharing = isSharing;
                deviceInfo.LastSeen = DateTime.Now;
                if (screenWidth > 0) deviceInfo.ScreenWidth = screenWidth;
                if (screenHeight > 0) deviceInfo.ScreenHeight = screenHeight;

                Console.WriteLine($"Device status updated: {deviceInfo.DeviceName} - Sharing: {isSharing}, Size: {screenWidth}x{screenHeight}");

                // Notify all viewers about the device update
                var deviceDict = deviceInfo.ToDictionary();
                await Clients.All.SendAsync("DeviceUpdated", deviceDict);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in UpdateDeviceStatus: {ex.Message}");
            throw;
        }
    }

    // Get list of available devices (called by viewer)
    public async Task GetDeviceList(string? companyId = null)
    {
        try
        {
            // Remove stale devices (not seen in 30 seconds)
            var now = DateTime.Now;
            var staleDevices = _devices.Where(kvp => (now - kvp.Value.LastSeen).TotalSeconds > 30).ToList();
            foreach (var stale in staleDevices)
            {
                _devices.TryRemove(stale.Key, out _);
            }

            // Filter devices by companyId if provided
            var devices = _devices.Values.AsEnumerable();
            if (!string.IsNullOrEmpty(companyId))
            {
                devices = devices.Where(d => d.CompanyId == companyId);
                Console.WriteLine($"Filtering devices by companyId: {companyId}");
            }
            else
            {
                devices = [];
            }

            // Convert to list of dictionaries
            var deviceList = devices
                .Select(d => d.ToDictionary())
                .ToList();

            Console.WriteLine($"Sending device list to viewer: {deviceList.Count} devices{(string.IsNullOrEmpty(companyId) ? "" : $" (filtered by companyId: {companyId})")}");
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveDeviceList", deviceList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in GetDeviceList: {ex.Message}");
            throw;
        }
    }

    // Request connection to a device (called by viewer)
    public async Task RequestConnection(string deviceConnectionId)
    {
        try
        {
            if (_devices.TryGetValue(deviceConnectionId, out var deviceInfo))
            {
                var viewerConnectionId = Context.ConnectionId;
                _connectionRequests[viewerConnectionId] = deviceConnectionId;

                Console.WriteLine($"Connection requested: Viewer {viewerConnectionId} -> Device {deviceInfo.DeviceName} ({deviceConnectionId})");

                // Notify the device about the connection request
                await Clients.Client(deviceConnectionId).SendAsync("ConnectionRequested", viewerConnectionId);
            }
            else
            {
                Console.WriteLine($"Device not found: {deviceConnectionId}");
                throw new Exception("Device not found");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in RequestConnection: {ex.Message}");
            throw;
        }
    }

    // Accept connection request (called by mobile device)
    public async Task AcceptConnection(string viewerConnectionId)
    {
        try
        {
            if (_devices.TryGetValue(Context.ConnectionId, out var deviceInfo))
            {
                Console.WriteLine($"Connection accepted: Device {deviceInfo.DeviceName} -> Viewer {viewerConnectionId}");

                // Notify the viewer that connection was accepted
                await Clients.Client(viewerConnectionId).SendAsync("ConnectionAccepted", deviceInfo.DeviceId);

                // Remove from pending requests
                _connectionRequests.TryRemove(viewerConnectionId, out _);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in AcceptConnection: {ex.Message}");
            throw;
        }
    }

    // Start sharing command (admin privilege - sent by viewer to device)
    public async Task StartSharing(string deviceConnectionId)
    {
        try
        {
            if (_devices.TryGetValue(deviceConnectionId, out var deviceInfo))
            {
                Console.WriteLine($"Start sharing command sent: Viewer {Context.ConnectionId} -> Device {deviceInfo.DeviceName} ({deviceConnectionId})");

                // Send start sharing command directly to the device
                await Clients.Client(deviceConnectionId).SendAsync("StartSharingCommand", Context.ConnectionId);

                Console.WriteLine($"Start sharing command delivered to device");
            }
            else
            {
                Console.WriteLine($"Device not found: {deviceConnectionId}");
                throw new Exception("Device not found");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in StartSharing: {ex.Message}");
            throw;
        }
    }

    // Called when a web client requests the current screen size
    public void RequestScreenSize()
    {
        try
        {
            lock (_screenSizeLock)
            {
                if (_lastScreenWidth > 0 && _lastScreenHeight > 0)
                {
                    Console.WriteLine($"Web client requested screen size, sending: {_lastScreenWidth}x{_lastScreenHeight}");
                    // Send to the requesting client
                    _ = Clients.Client(Context.ConnectionId).SendAsync("ReceiveScreenSize", _lastScreenWidth, _lastScreenHeight);
                }
                else
                {
                    Console.WriteLine("Web client requested screen size, but no size is stored yet");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in RequestScreenSize: {ex.GetType().Name}: {ex.Message}");
        }
    }

    // Called when a web client sends a control command
    public async Task SendControlCommand(string commandType, double x, double y, string? action = null)
    {
        try
        {
            Console.WriteLine($"Control command received: {commandType} at ({x}, {y}), action: {action ?? "none"}");

            // Broadcast control command to all connected Flutter clients
            // Flutter app will handle executing the command
            await Clients.All.SendAsync("ReceiveControlCommand", commandType, x, y, action);

            Console.WriteLine($"Control command forwarded to Flutter clients");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in SendControlCommand: {ex.GetType().Name}: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    // Called when Flutter client sends device screen dimensions
    public async Task SendScreenSize(int width, int height)
    {
        try
        {
            Console.WriteLine($"Screen size received: {width}x{height}");

            // Store the screen size for newly connecting clients
            lock (_screenSizeLock)
            {
                _lastScreenWidth = width;
                _lastScreenHeight = height;
            }

            // Broadcast to all connected clients
            await Clients.All.SendAsync("ReceiveScreenSize", width, height);
            Console.WriteLine($"Screen size forwarded to web clients: {width}x{height}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in SendScreenSize: {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
        if (exception != null)
        {
            Console.WriteLine($"Disconnection error: {exception.Message}");
        }

        // Remove device from registry if it was registered
        if (_devices.TryRemove(Context.ConnectionId, out var deviceInfo))
        {
            Console.WriteLine($"Device removed: {deviceInfo.DeviceName}");
            // Notify all viewers about device removal
            await Clients.All.SendAsync("DeviceRemoved", Context.ConnectionId);
        }

        // Remove any pending connection requests
        var requestsToRemove = _connectionRequests.Where(kvp => kvp.Key == Context.ConnectionId || kvp.Value == Context.ConnectionId).ToList();
        foreach (var request in requestsToRemove)
        {
            _connectionRequests.TryRemove(request.Key, out _);
        }

        await base.OnDisconnectedAsync(exception);
    }
}

