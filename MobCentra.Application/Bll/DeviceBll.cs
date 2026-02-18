using Microsoft.Extensions.Configuration;
using MobCentra.Application.Dto;
using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for device management operations including registration, commands, notifications, and tracking
    /// </summary>
    public class DeviceBll(IBaseDal<Device, Guid, DeviceFilter> baseDal,IDevicesGeoFenceLogBll devicesGeoFenceLogBll, INotificationBll notificationBll, IDeviceBatteryTransBll deviceBatteryTransBll, IDeviceTransactionBll deviceTransactionBll, IEmailSender emailSender, IConstraintBll constraintBll, IVersionBll versionBll, Lazy<ICompanyBll> companyBll, Lazy<IGroupBll> groupBll, Lazy<IProfileBll> profileBll, Lazy<IDeviceApplicationBll> deviceApplicationBll, ISettingBll settingBll, IConfiguration configuration, IGeoFencBll geoFencBll, IGeoFencSettingBll geoFencSettingBll, IDeviceLogBll deviceLogBll, ICompanySubscriptionBll companySubscriptionBll, IDeviceQueuBll deviceQueuBll) : BaseBll<Device, Guid, DeviceFilter>(baseDal), IDeviceBll
    {

        /// <summary>
        /// Adds a new device or updates existing device if device code already exists
        /// Validates subscription, checks device limits, and sends initial sync commands
        /// </summary>
        /// <param name="entity">The device entity to add or update</param>
        public override async Task AddAsync(Device entity)
        {
            if (!await base.IsAuthorizedAsync(Guid.Parse(Permissions.QrCode)))
            {
                throw new UnauthorizedAccessException("لا تمتلك صلاحية تسجيل جهاز");
            }
            // Check if company has reached the maximum number of devices limit
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfDevices);
            //if (entity.TrackActivated == 1)
            //{
            //    await CheckTrackedLicenseAsync(entity);
            //}
            // Disable tracking by default
            entity.TrackActivated = 0;

            // Check if device with same code already exists
            var record = await base.FindByExpressionAsync(x => x.Code == entity.Code);
            if (record is not null)
            {
                // Validate subscription for existing device
                bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(record.CompanyId.Value);
                if (!isValidSubscription)
                    throw new Exception("لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام");
                if (record.DeviceDateTime.HasValue)
                {
                    var dt = record.DeviceDateTime.Value;
                    record.DeviceDateTime = dt.ToUniversalTime();
                }
                // Preserve existing device properties if new values are not provided
                entity.BatteryPercentage ??= record.BatteryPercentage;
                entity.CurrentLocation ??= record.CurrentLocation;
                entity.Name ??= record.Name;
                entity.Code ??= record.Code;
                entity.Id = record.Id;
                entity.Token ??= record.Token;
                entity.CreatedBy = record.CreatedBy;
                entity.CreatedDate = record.CreatedDate;
                entity.CompanyId ??= record.CompanyId;
                entity.GroupId ??= record.GroupId;
                entity.TotalSpace ??= record.TotalSpace;
                entity.UsedSpace ??= record.UsedSpace;
                entity.FreeSpace ??= record.FreeSpace;
                entity.OSVersion ??= record.OSVersion;
                entity.DeviceModel ??= record.DeviceModel;
                entity.DeviceName ??= record.DeviceName;
                entity.ScreenSize ??= record.ScreenSize;
                entity.IMEI ??= record.IMEI;
                entity.BatteryCapacity ??= record.BatteryCapacity;
                entity.ProfileId ??= record.ProfileId;
                entity.IsOnline = 1;
                entity.ImagesSpace ??= record.ImagesSpace;
                entity.VideosSpace ??= record.VideosSpace;
                entity.AudioSpace ??= record.AudioSpace;
                entity.DocumentsSpace ??= record.DocumentsSpace;
                entity.OtherSpace ??= record.OtherSpace;
                entity.SystemSpace ??= record.SystemSpace;
                entity.AppVersion ??= record.AppVersion;
                entity.BatteryDate ??= record.BatteryDate;
                entity.DeviceDateTime ??= record.DeviceDateTime.HasValue ? record.DeviceDateTime.Value.ToUniversalTime(): record.DeviceDateTime;
                entity.GeoFencDate ??= record.GeoFencDate;
                entity.TrackActivated ??= record.TrackActivated;
                await base.UpdateAsync(entity);
            }
            else
            {
                // Validate subscription for new device
                bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(entity.CompanyId.Value);
                if (!isValidSubscription)
                    throw new Exception("لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام");

                // Mark device as online and add new device
                entity.IsOnline = 1;
                if (entity.DeviceDateTime.HasValue)
                {
                    var dt = entity.DeviceDateTime.Value;
                    entity.DeviceDateTime = dt.ToUniversalTime();
                }

                await base.AddAsync(entity);
            }

            // Send initial sync commands to device
            await SendCommandAsync(new SendCommandDto { Command = "syncDeviceInfo", Token = [entity.Token] });
            await SendCommandAsync(new SendCommandDto { Command = "getStorageInfo", Token = [entity.Token] });

        }

        /// <summary>
        /// Uploads an image file and sends a command to change device wallpaper
        /// </summary>
        /// <param name="imageDto">DTO containing the image file and device token</param>
        /// <returns>Response indicating success or failure</returns>
        public async Task<DcpResponse<bool>> UploadImageAndSendCommandAsync(ImageDto imageDto)
        {
            if (imageDto is null)
                return new DcpResponse<bool>(false, "الرجاء المحاولة لاحقاً", false);
            if (string.IsNullOrEmpty(imageDto.Image))
                return new DcpResponse<bool>(false, "Image content (base64) is required.", false);
            var (isValid, errorMessage) = ValidateImageSignature(imageDto.Image);
            if (!isValid)
                return new DcpResponse<bool>(false, errorMessage, false);
            // Upload image file and get the file path
            var imagePath = await imageDto.Image.UplodaFiles(".png", "images", Guid.NewGuid().ToString());
            await SendCommandAsync(new SendCommandDto { Command = "changeWallPaper", WallpaperUrl = imagePath, Token = imageDto.Token });
            return new DcpResponse<bool>(true);
        }

        /// <summary>
        /// Validates that the base64 content is a real image file by checking binary signatures (JPEG, PNG, GIF, BMP, WebP).
        /// </summary>
        private static (bool isValid, string errorMessage) ValidateImageSignature(string base64)
        {
            string data = base64.Trim();
            if (data.Length == 0) return (false, "Image content is empty.");
            int commaIndex = data.IndexOf(',');
            if (commaIndex >= 0 && data.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                data = data[(commaIndex + 1)..].Trim();
            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(data);
            }
            catch (FormatException)
            {
                return (false, "Invalid base64 image content.");
            }
            if (bytes.Length < 4)
                return (false, "File is not a valid image. Content is too short.");
            // JPEG: FF D8 FF
            if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
                return (true, null!);
            // PNG: 89 50 4E 47 0D 0A 1A 0A
            if (bytes.Length >= 8 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47
                && bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A)
                return (true, null!);
            // GIF87a or GIF89a: 47 49 46 38 37/39 61
            if (bytes.Length >= 6 && bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x38
                && (bytes[4] == 0x37 || bytes[4] == 0x39) && bytes[5] == 0x61)
                return (true, null!);
            // BMP: 42 4D
            if (bytes.Length >= 2 && bytes[0] == 0x42 && bytes[1] == 0x4D)
                return (true, null!);
            // WebP: RIFF....WEBP (52 49 46 46 ... 57 45 42 50 at offset 8)
            if (bytes.Length >= 12 && bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46
                && bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50)
                return (true, null!);
            return (false, "File is not a valid image. Only JPEG, PNG, GIF, BMP, or WebP formats are allowed.");
        }
        public async Task<DcpResponse<bool>> UploadFileAndSendCommandAsync(ImageDto imageDto)
        {
            const string allowedExtension = ".apk";
            if (imageDto is null)
                return new DcpResponse<bool>(false, "الرجاء المحاولة لاحقاً", false);
            var fileName = imageDto.Name?.Trim();
            if (string.IsNullOrEmpty(fileName) || !fileName.EndsWith(allowedExtension, StringComparison.OrdinalIgnoreCase))
                return new DcpResponse<bool>(false, "Only APK files are allowed. The file name must end with .apk", false);
            if (string.IsNullOrEmpty(imageDto.Image))
                return new DcpResponse<bool>(false, "File content (base64) is required.", false);
            var (isValid, errorMessage) = ValidateApkSignature(imageDto.Image);
            if (!isValid)
                return new DcpResponse<bool>(false, errorMessage, false);
            // Upload file and get the file path
            var imagePath = await imageDto.Image.UplodaFiles(type: imageDto.Type, name: Guid.NewGuid().ToString());
            string fullName = $"http://mobcentra.com/assets/applications/{imagePath}";
            await SendCommandAsync(new SendCommandDto { Command = "silent_download", FilePath = imageDto.Path, FileUrl = fullName, FileName = imageDto.Name, Token = imageDto.Token });
            return new DcpResponse<bool>(true);
        }

        /// <summary>
        /// Validates that the base64 content is a real APK (ZIP) file by checking the binary signature.
        /// APK files are ZIP archives; ZIP magic bytes are 0x50 0x4B (PK) followed by a valid ZIP header.
        /// </summary>
        private static (bool isValid, string errorMessage) ValidateApkSignature(string base64)
        {
            const byte ZipFirst = 0x50;  // 'P'
            const byte ZipSecond = 0x4B; // 'K'
            // Standard ZIP local file header, end of central dir, or spanned archive
            static bool IsValidZipHeader(byte b2, byte b3) =>
                (b2 == 0x03 && b3 == 0x04) || (b2 == 0x05 && b3 == 0x06) || (b2 == 0x07 && b3 == 0x08);

            string data = base64.Trim();
            if (data.Length == 0) return (false, "File content is empty.");
            // Strip optional data URL prefix (e.g. data:application/vnd.android.package-archive;base64,)
            int commaIndex = data.IndexOf(',');
            if (commaIndex >= 0 && data.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                data = data[(commaIndex + 1)..].Trim();
            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(data);
            }
            catch (FormatException)
            {
                return (false, "Invalid base64 file content.");
            }
            if (bytes.Length < 4)
                return (false, "File is not a valid APK. Content is too short.");
            if (bytes[0] != ZipFirst || bytes[1] != ZipSecond)
                return (false, "File is not a valid APK. Only APK (Android package) format is allowed.");
            if (!IsValidZipHeader(bytes[2], bytes[3]))
                return (false, "File is not a valid APK. Invalid file signature.");
            return (true, null!);
        }
        /// <summary>
        /// Retrieves devices with filtering, pagination, and online status updates based on last seen time
        /// </summary>
        /// <param name="searchParameters">Filter parameters including keyword, group, status, profile, and date filters</param>
        /// <returns>Paginated result containing matching devices with updated online status</returns>
        public override async Task<PageResult<Device>> GetAllAsync(DeviceFilter searchParameters)
        {

            // Build search expression with multiple filter criteria
            if (searchParameters is not null)
            {
                // Set default values for status filters (-1 means no filter)
                searchParameters.StatusId ??= -1;
                searchParameters.PinnedStatusId ??= -1;
                searchParameters.Expression = new Func<Device, bool>(a => a.CompanyId == searchParameters.CompanyId
                && (searchParameters.Keyword.IsNullOrEmpty() || a.Name.Contains(searchParameters?.Keyword))
                && (searchParameters.GroupId == null || a.GroupId == searchParameters.GroupId)
                && (searchParameters.StatusId == -1 || (a.IsOnline == searchParameters.StatusId))
                && (searchParameters.VersionNo == null || (a.AppVersion == searchParameters.VersionNo))
                && (searchParameters.Percentage == "0" || searchParameters.Percentage.IsNullOrEmpty() || (Convert.ToInt16(a.BatteryPercentage) <= Convert.ToInt16(searchParameters.Percentage)))
                && (searchParameters.PinnedStatusId == -1 || ((searchParameters.PinnedStatusId == 1 && !a.UnpinedDate.HasValue) || (searchParameters.PinnedStatusId == 0 && a.UnpinedDate.HasValue)))
                && (searchParameters.ProfileId == null || a.ProfileId == searchParameters.ProfileId)
                && (!searchParameters.FromDate.HasValue || (a.UnpinedDate.HasValue && a.UnpinedDate.Value.Date == searchParameters.FromDate.Value.Date))
                );
            }
            var data = await base.GetAllAsync(searchParameters);

            // Get refresh time setting to determine online status
            var setting = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.RefreshTime" && a.CompanyId == searchParameters.CompanyId);
            var deviceTimeMargin = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.CheckTimeMargin" && a.CompanyId == searchParameters.CompanyId);

            // Update online status based on last seen time
            if (setting != null)
            {
                foreach (var device in data.Collections)
                {

                    if (device.LastSeenDate == null) continue;

                    DateTime lastSeenDate = device.LastSeenDate.Value;

                    if (deviceTimeMargin is not null)
                    {
                        if (device.DeviceDateTime is null)
                        {
                            device.IsWrongTime = true;
                        }
                        else
                        {
                            device.IsWrongTime = Math.Abs(lastSeenDate.Subtract(device.DeviceDateTime.Value).TotalSeconds) > int.Parse(deviceTimeMargin.SettingValue);
                        }
                    }
                    // Calculate seconds since device was last seen
                    int secondsSinceLastSeen = (int)(DateTime.UtcNow - lastSeenDate).TotalSeconds;

                    // Mark device as offline if it hasn't been seen within refresh time + 10 seconds buffer
                    if (secondsSinceLastSeen > Convert.ToInt16(setting.SettingValue) + 10)
                    {
                        device.IsOnline = 0;
                        await base.UpdateAsync(device);
                    }
                    else
                    {
                        device.IsOnline = 1;
                        await base.UpdateAsync(device);
                    }
                }

            }
            if (searchParameters.IsWrongTime != -1 && searchParameters.IsWrongTime != null)
            {
                data.Collections = [.. data.Collections.Where(a => a.IsWrongTime == Convert.ToBoolean(searchParameters.IsWrongTime))];
            }
            return data;
        }

        /// <summary>
        /// Checks company settings including groups, profiles, device count limits, and subscription status
        /// </summary>
        /// <param name="companyId">The company identifier</param>
        /// <returns>Response containing settings status information</returns>
        public async Task<DcpResponse<object>> CheckSettingsAsync(Guid companyId)
        {
            // Get counts for groups, devices, and profiles
            var groupsCount = (await groupBll.Value.GetAllAsync(new GroupFilter { CompanyId = companyId })).Count;
            var devicesCount = (await base.GetAllAsync(new DeviceFilter { CompanyId = companyId })).Count;
            var profilesCount = (await profileBll.Value.GetAllAsync(new ProfileFilter { CompanyId = companyId })).Count;

            // Get maximum allowed devices and check subscription status
            var maxCount = (await companyBll.Value.GetByIdAsync(companyId)).NoOfDevices ?? 0;
            bool isExpired = await companySubscriptionBll.IsValidSubscriptionAsync(companyId);

            return new DcpResponse<object>(new { hasGroups = groupsCount > 0, hasProfiles = profilesCount > 0, hasExeced = devicesCount >= maxCount, isExpired });
        }

        /// <summary>
        /// Gets the count of devices that need to be updated to the latest app version
        /// </summary>
        /// <param name="companyId">The company identifier</param>
        /// <returns>Response containing the count of devices needing updates</returns>
        public async Task<DcpResponse<dynamic>> GetVersionCountAsync(Guid companyId)
        {
            // Get the latest app version
            var versions = await versionBll.GetAllAsync();
            var lastVersion = versions.LastOrDefault() ?? new Domain.Entities.Version() { VersionNumber = "1.0.0" };

            var settings = await settingBll.FindByExpressionAsync(a => a.CompanyId == companyId && a.SettingName == "DCP.EnableRemoteSupport");

            // Count devices that are not on the latest version and are still active (not unpinned)
            int allDevices = await base.GetCountByExpressionAsync(x => x.CompanyId == companyId && x.AppVersion != lastVersion.VersionNumber && x.UnpinedDate == null);
            return new DcpResponse<dynamic>(new { Count = allDevices , RemoteEnable = settings != null ? settings.SettingValue : "0" , lastVersion = lastVersion.VersionNumber });
        }

        /// <summary>
        /// Sends a command to one or more devices via Google MDM API
        /// Handles device validation, subscription checks, offline status, and command queuing
        /// </summary>
        /// <param name="sendCommandDto">DTO containing command details, tokens, and optional parameters</param>
        /// <returns>Response indicating success or error message</returns>
        public async Task<DcpResponse<string>> SendCommandAsync(SendCommandDto sendCommandDto)
        {
            try
            {
                Device device = null;
                // Validate device if token is provided
                if (sendCommandDto.Token.Length > 0)
                {
                    device = await FindByExpressionAsync(x => x.Token == sendCommandDto.Token[0]);

                    if (device == null)
                        return new DcpResponse<string>(null, "الجهاز غير معرف", false);

                    // Validate subscription before sending command
                    bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(device.CompanyId.Value);
                    if (!isValidSubscription)
                        return new DcpResponse<string>(null, "لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام", false);
                }

                // Check if device is online before sending command
                (bool flowControl, DcpResponse<string> value) = await HandleOfflineOnlineStatus(sendCommandDto, device);
                if (!flowControl)
                {
                    return value;
                }

                // Initialize Google Command Sender
                string path = configuration.GetSection("adminSdkPath").Value;
                var googleCommandSender = new GoogleCommandSender(path, "mdmapp-4bc4a");
                string[] packages = [];

                // Get blocked packages for blacklist command
                if (sendCommandDto.Command == "blacklist_settings")
                {
                    packages = (await deviceApplicationBll.Value.FindAllByExpressionAsync(a => a.DeviceId == device.Id && (a.IsBlocked ?? false))).Select(a => a.PackgeName).ToArray();
                }

                // Get allowed packages for whitelist command
                if (sendCommandDto.Command == "whitelist_settings")
                {
                    packages = (await deviceApplicationBll.Value.FindAllByExpressionAsync(a => a.DeviceId == device.Id && !(a.IsBlocked ?? false))).Select(a => a.PackgeName).ToArray();
                }

                // Handle device unbinding
                if (sendCommandDto.Command == "unbind_device")
                {
                    device.UnpinedDate = DateTime.UtcNow;
                    device.Token = null;
                    device.Code = null;
                    await base.UpdateAsync(device);
                }

                // Expand tokens if group ID is provided
                if ((sendCommandDto.GroupId ?? Guid.Empty) != Guid.Empty)
                {
                    var devices = (await FindAllByExpressionAsync(x => x.GroupId == sendCommandDto.GroupId)).Select(a => a.Token).ToArray();
                    sendCommandDto.Token = devices;
                }

                // Expand tokens if company ID is provided
                if ((sendCommandDto.CompanyId ?? Guid.Empty) != Guid.Empty)
                {
                    var devices = (await FindAllByExpressionAsync(x => x.CompanyId == sendCommandDto.CompanyId)).Select(a => a.Token).ToArray();
                    sendCommandDto.Token = devices;
                }

                // Set APK URL for internal installations
                if (sendCommandDto.IsInternal)
                {
                    var setting = (await settingBll.FindByExpressionAsync(x => x.SettingName == "DCP.MDM.URL"));
                    sendCommandDto.ApkUrl = setting.SettingValue;
                }

                // Send command to each device token
                foreach (var token in sendCommandDto.Token)
                {
                    try
                    {
                        await googleCommandSender.SendCommandAsync(token, sendCommandDto.Command, packages, sendCommandDto.ApkUrl, sendCommandDto.Password, sendCommandDto.PackageName, sendCommandDto.WallpaperUrl, sendCommandDto.IsInternal, sendCommandDto.FilePath, sendCommandDto.FileName, sendCommandDto.FromDate, sendCommandDto.ToDate, sendCommandDto.FileUrl);
                        // Log the command for audit purposes
                        await HandleDeviceLog(sendCommandDto, token, packages);
                    }
                    catch 
                    {
                        continue;
                    }
                }
                return new DcpResponse<string>(string.Empty, "");
            }
            catch (Exception ex)
            {
                return new DcpResponse<string>(ex.Message, ex.InnerException?.Message ?? ex.Message, false);
            }
        }

        /// <summary>
        /// Checks if device is online based on last seen time and refresh time setting
        /// Updates device online status accordingly
        /// </summary>
        /// <param name="sendCommandDto">The command DTO (not used but kept for consistency)</param>
        /// <param name="device">The device to check</param>
        /// <returns>Tuple indicating if command should proceed and any error response</returns>
        private async Task<(bool flowControl, DcpResponse<string> value)> HandleOfflineOnlineStatus(SendCommandDto sendCommandDto, Device device)
        {
            try
            {
                var setting = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.RefreshTime" && a.CompanyId == device.CompanyId);

                if (setting == null) return (true, null);

                if (device.LastSeenDate == null) return (flowControl: false, value: new DcpResponse<string>(null, "الجهاز مغلق حالياُ", false));

                DateTime lastSeenDate = device.LastSeenDate.Value;

                int secondsSinceLastSeen = (int)(DateTime.UtcNow - lastSeenDate).TotalSeconds;
                if (secondsSinceLastSeen > Convert.ToInt16(setting.SettingValue) + 10)
                {
                    device.IsOnline = 0;
                    await base.UpdateAsync(device);
                    return (flowControl: false, value: new DcpResponse<string>(null, "الجهاز مغلق حالياُ", false));
                }
                else
                {
                    device.IsOnline = 1;
                    await base.UpdateAsync(device);
                    return (flowControl: true, value: null);
                }
            }
            catch (Exception ex)
            {
                return (flowControl: false, value: new DcpResponse<string>(null, ex.InnerException?.Message ?? ex.Message, false));
            }
        }

        public override async Task<Device> GetByIdAsync(Guid id)
        {
            var device = await base.GetByIdAsync(id);
            if (device.LastSeenDate == null) return device;

            DateTime lastSeenDate = device.LastSeenDate.Value;
            var deviceTimeMargin = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.CheckTimeMargin" && a.CompanyId == device.CompanyId);

            if (deviceTimeMargin is not null)
            {
                if (device.DeviceDateTime is null)
                {
                    device.IsWrongTime = true;
                }
                else
                {
                    device.IsWrongTime = Math.Abs(lastSeenDate.Subtract(device.DeviceDateTime.Value).TotalSeconds) > int.Parse(deviceTimeMargin.SettingValue);
                }
            }
            return device;
        }

        /// <summary>
        /// Logs device command execution for audit purposes
        /// </summary>
        /// <param name="sendCommandDto">The command DTO containing command details</param>
        /// <param name="token">The device token</param>
        /// <param name="packages">Array of package names if applicable</param>
        private async Task HandleDeviceLog(SendCommandDto sendCommandDto, string token, string[] packages)
        {
            var device = await FindByExpressionAsync(x => x.Token == token);
            if (device is not null)
            {
                Dictionary<string, string> keyValuePairs = [];

                if (!sendCommandDto.Password.IsNullOrEmpty())
                    keyValuePairs.Add(nameof(sendCommandDto.Password), sendCommandDto.Password);

                if (!sendCommandDto.ApkUrl.IsNullOrEmpty())
                    keyValuePairs.Add(nameof(sendCommandDto.ApkUrl), sendCommandDto.ApkUrl);

                if (packages.Length > 0)
                    keyValuePairs.Add(nameof(packages), string.Join(",", packages.Select(a => a)));

                await deviceLogBll.AddAsync(new DeviceLog { CommandName = sendCommandDto.Command, Data = JsonConvert.SerializeObject(keyValuePairs), DeviceId = device.Id, Device = null });
            }
        }
        /// <summary>
        /// Sends push notification to one or more devices
        /// Supports sending to individual devices, groups, or entire company
        /// </summary>
        /// <param name="sendNotifyDto">DTO containing notification details and target devices</param>
        /// <returns>Response indicating success or error message</returns>
        public async Task<DcpResponse<string>> SendNotifyAsync(SendNotifyDto sendNotifyDto)
        {
            try
            {
                // bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(sendNotifyDto.CompanyId.Value);
                // if (!isValidSubscription)
                //     return new DcpResponse<string>(null, "لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام", false);


                string path = configuration.GetSection("adminSdkPath").Value;
                var googleCommandSender = new GoogleCommandSender(path, "mdmapp-4bc4a");
                if ((sendNotifyDto.GroupId ?? Guid.Empty) != Guid.Empty)
                {
                    var devices = (await FindAllByExpressionAsync(x => x.GroupId == sendNotifyDto.GroupId)).Select(a => a.Token).ToArray();
                    sendNotifyDto.Token = devices;
                }
                if ((sendNotifyDto.CompanyId ?? Guid.Empty) != Guid.Empty)
                {
                    var devices = (await FindAllByExpressionAsync(x => x.CompanyId == sendNotifyDto.CompanyId)).Select(a => a.Token).ToArray();
                    sendNotifyDto.Token = devices;
                }

                foreach (var token in sendNotifyDto.Token)
                {
                    try
                    {
                        await googleCommandSender.SendNotifyAsync(token, sendNotifyDto.Title, sendNotifyDto.Body);
                        await notificationBll.AddAsync(new() { Body = sendNotifyDto.Body, CompanyId = sendNotifyDto.CurrentCompanyId.Value, Token = token, Title = sendNotifyDto.Title });
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                return new DcpResponse<string>(string.Empty, "");
            }
            catch (Exception ex)
            {
                return new DcpResponse<string>(ex.Message, ex.InnerException?.Message ?? ex.Message, false);
            }
        }
        /// <summary>
        /// Updates device information with comprehensive handling of subscription, queue, email notifications, tracking, and data updates
        /// </summary>
        /// <param name="entity">The device entity with updated information</param>
        public override async Task UpdateAsync(Device entity)
        {
            var record = await base.FindByExpressionAsync(x => x.Token == entity.Token || entity.Code == x.Code);
            await CheckSubscriptionAsync(record);
            await AddToQueuAsync(entity, record);
            await HandleQueuAsync(record);
            await HandelEmailNofication(entity, record);
            await HandleTracking(entity, record);
            await UpdateDataAsync(entity, record);
            await HandleGeoFencAsync(record);
        }

        private async Task HandleGeoFencAsync(Device record)
        {
            try
            {
                var geoFenc = await geoFencBll.FindByExpressionAsync(a => a.DeviceId == record.Id);
                if (geoFenc != null)
                {
                    var reader = new WKTReader();
                    var swappedPoint = new Point(record.CurrentLocation.Y, record.CurrentLocation.X)
                    {
                        SRID = record.CurrentLocation.SRID
                    };
                    bool isInside = geoFenc.Area.Contains(swappedPoint);
                    DateTime toDay = DateTime.UtcNow;
                    int type = isInside ? 1 : 0;
                    DevicesGeoFenceLog devicesGeoFenceLog = await devicesGeoFenceLogBll.FindLastByExpressionAsync(a => a.DeviceId == record.Id && a.TransType == type);
                    if (devicesGeoFenceLog == null)
                    {
                        await devicesGeoFenceLogBll.AddAsync(new DevicesGeoFenceLog { TransDate = DateTime.UtcNow, Coordinations = record.CurrentLocation, TransType = type, DeviceId = record.Id, Device = null });
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Updates device data properties while preserving existing values if new values are not provided
        /// Also handles last seen date based on update source
        /// </summary>
        /// <param name="entity">The updated device entity</param>
        /// <param name="record">The existing device record from database</param>
        private async Task UpdateDataAsync(Device entity, Device record)
        {
            if (entity.DeviceDateTime.HasValue)
            {
                var dt = entity.DeviceDateTime.Value;
                entity.DeviceDateTime = dt.ToUniversalTime();
                //record.DeviceDateTime = dt.Kind switch
                //{
                //    DateTimeKind.Utc => dt,
                //    DateTimeKind.Local => dt.ToUniversalTime(),
                //    DateTimeKind.Unspecified => DateTime.SpecifyKind(dt, DateTimeKind.Local).ToUniversalTime(),
                //    _ => dt
                //};
            }
            string dbValue = entity.BatteryPercentage;
            entity.BatteryPercentage ??= record.BatteryPercentage;
            entity.CurrentLocation ??= record.CurrentLocation;
            entity.Name ??= record.Name;
            entity.Code ??= record.Code;
            entity.Id = record.Id;
            entity.Token ??= record.Token;
            entity.CreatedBy = record.CreatedBy;
            entity.CreatedDate = record.CreatedDate;
            entity.CompanyId ??= record.CompanyId;
            entity.GroupId ??= record.GroupId;
            entity.TotalSpace ??= record.TotalSpace;
            entity.UsedSpace ??= record.UsedSpace;
            entity.FreeSpace ??= record.FreeSpace;
            entity.OSVersion ??= record.OSVersion;
            entity.DeviceModel ??= record.DeviceModel;
            entity.DeviceName ??= record.DeviceName;
            entity.ScreenSize ??= record.ScreenSize;
            entity.IMEI ??= record.IMEI;
            entity.BatteryCapacity ??= record.BatteryCapacity;
            entity.ProfileId ??= record.ProfileId;
            entity.IsOnline = 1;
            entity.ImagesSpace ??= record.ImagesSpace;
            entity.VideosSpace ??= record.VideosSpace;
            entity.AudioSpace ??= record.AudioSpace;
            entity.DocumentsSpace ??= record.DocumentsSpace;
            entity.OtherSpace ??= record.OtherSpace;
            entity.SystemSpace ??= record.SystemSpace;
            entity.AppVersion ??= record.AppVersion;
            entity.BatteryDate ??= record.BatteryDate;
            entity.DeviceDateTime ??= record.DeviceDateTime;
            entity.GeoFencDate ??= record.GeoFencDate;
            entity.TrackActivated ??= record.TrackActivated;
            if (!entity.IsFromBackOffice)
            {
                entity.LastSeenDate = DateTime.UtcNow;
            }
            else
            {
                entity.LastSeenDate ??= record.LastSeenDate;
            }
            await base.UpdateAsync(entity);

            if (entity.IsFromBackOffice) return;
            await HandleBatteryData(dbValue, record);

        }

        /// <summary>
        /// Handles battery data tracking by creating battery transactions when percentage changes
        /// Also cleans up old battery transactions older than 3 days
        /// </summary>
        /// <param name="entity">The updated device entity</param>
        /// <param name="record">The existing device record</param>
        private async Task HandleBatteryData(string dbValue, Device record)
        {
            DateTime past3Days = DateTime.UtcNow.AddDays(-3);

            var allTrans = await deviceBatteryTransBll.FindAllByExpressionAsync(a => a.TransDateTime <= past3Days);

            if (allTrans != null && allTrans.Count > 0)
            {
                await deviceBatteryTransBll.DeleteRangeAsync(allTrans);
            }
            if (dbValue != record.BatteryPercentage)
            {
                await deviceBatteryTransBll.AddAsync(new DeviceBatteryTrans { BatteryPercentage = record.BatteryPercentage, DeviceId = record.Id, TransDateTime = DateTime.UtcNow });
            }
        }

        /// <summary>
        /// Adds device to queue for profile refresh if profile was changed from back office
        /// </summary>
        /// <param name="entity">The updated device entity</param>
        /// <param name="record">The existing device record</param>
        private async Task AddToQueuAsync(Device entity, Device record)
        {
            if (record.ProfileId != entity.ProfileId && entity.IsFromBackOffice && await deviceQueuBll.FindByExpressionAsync(a => a.DeviceId == record.Id) is null)
            {
                await deviceQueuBll.AddAsync(new DeviceQueu { DeviceId = record.Id });
            }
        }

        /// <summary>
        /// Validates that the company subscription is still active
        /// </summary>
        /// <param name="record">The device record to check subscription for</param>
        /// <exception cref="Exception">Thrown if subscription is not valid</exception>
        private async Task CheckSubscriptionAsync(Device record)
        {
            if (!record.CompanyId.HasValue) return;
            bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(record.CompanyId.Value);
            if (!isValidSubscription)
                throw new Exception("لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام");
        }

        /// <summary>
        /// Handles device location tracking by creating transaction records with distance calculations
        /// Only tracks during configured time windows if tracking is activated
        /// </summary>
        /// <param name="entity">The updated device entity</param>
        /// <param name="record">The existing device record</param>
        /// <returns>True if tracking was processed, false otherwise</returns>
        private async Task<bool> HandleTracking(Device entity, Device record)
        {
            if (record.TrackActivated == 1 && !entity.IsFromBackOffice && false)//ToDO
            {
                var setting = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.TrackingTime" && a.CompanyId == record.CompanyId);
                if (setting is not null)
                {
                    TimeSpan start = DateTime.ParseExact(setting.SettingValue, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                    TimeSpan end = DateTime.ParseExact(setting.SettingValueOt, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                    TimeSpan now = DateTime.UtcNow.TimeOfDay;
                    if (!(now >= start && now <= end)) return false;
                }

                string location = $"{record.CurrentLocation.X},{record.CurrentLocation.Y}";
                var deviceTransaction = new DeviceTransaction();
                deviceTransaction.DeviceId = record.Id;
                deviceTransaction.Device = null;
                if (record.CurrentLocation != null)
                {
                    deviceTransaction.Coordinations = location;
                }
                deviceTransaction.TransDateTime = DateTime.UtcNow;
                deviceTransaction.Distance = 0;

                var lastTransaction = await deviceTransactionBll.FindLastByExpressionAsync(x => x.DeviceId == record.Id);
                if (lastTransaction != null && !deviceTransaction.Coordinations.IsNullOrEmpty() && !lastTransaction.Coordinations.IsNullOrEmpty())
                {

                    string[] dbCoordination = lastTransaction.Coordinations.Split(',');
                    string[] currentCoordination = location.Split(',');

                    deviceTransaction.Distance = Haversine(
                            double.Parse(dbCoordination.First().Trim()),
                            double.Parse(dbCoordination.Last().Trim()),
                            double.Parse(currentCoordination.First().Trim()),
                            double.Parse(currentCoordination.Last().Trim()));
                }
                await deviceTransactionBll.AddAsync(deviceTransaction);
            }

            return true;
        }

        /// <summary>
        /// Handles email notifications for battery warnings and geofence violations
        /// Only processes notifications for device updates (not back office updates)
        /// </summary>
        /// <param name="entity">The updated device entity</param>
        /// <param name="record">The existing device record</param>
        private async Task HandelEmailNofication(Device entity, Device record)
        {
            try
            {
                if (!entity.IsFromBackOffice)
                {
                    var setting = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.BatteyWarningLevel" && a.CompanyId == record.CompanyId);
                    var toEmail = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.Notification.Email" && a.CompanyId == record.CompanyId);

                    if (setting != null && toEmail != null)
                    {
                        await HandleBatteryNotifyAsync(record, setting, toEmail);
                        await HandleGeoFencNotifyAsync(record, toEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Checks if device has moved outside its geofence and sends notification or executes command
        /// </summary>
        /// <param name="record">The device record to check</param>
        /// <param name="toEmail">The email setting for notifications</param>
        private async Task HandleGeoFencNotifyAsync(Device record, Setting toEmail)
        {
            if (record.GeoFencDate.HasValue && DateTime.UtcNow.Date == record.GeoFencDate.Value.Date) return;

            var geoFenc = await geoFencBll.FindByExpressionAsync(a => a.DeviceId == record.Id);
            if (geoFenc != null)
            {
                record.CurrentLocation.SRID = geoFenc.Area.SRID;
                var geoFencSetting = await geoFencSettingBll.FindLastByExpressionAsync(a => a.CompanyId == record.CompanyId);
                var reader = new WKTReader();
                var swappedPoint = new Point(record.CurrentLocation.Y, record.CurrentLocation.X)
                {
                    SRID = record.CurrentLocation.SRID
                };
                bool isInside = geoFenc.Area.Contains(swappedPoint);
                if (!isInside)
                {

                    record.GeoFencDate = DateTime.UtcNow;

                    if (geoFencSetting == null)
                    {
                        await HandleGeFencEmailAsync(record, toEmail);
                    }
                    else
                    {
                        await HandleGeFencCommandAsync(record, geoFencSetting, toEmail);
                    }
                }
            }
        }

        /// <summary>
        /// Sends email notification when device moves outside geofence
        /// </summary>
        /// <param name="record">The device record that violated geofence</param>
        /// <param name="toEmail">The email setting containing recipient address</param>
        private async Task HandleGeFencEmailAsync(Device record, Setting toEmail)
        {
            string body = $@"Dear Sir/Madam,
<br/>
This is an automated notification from Mobcentra.
<br/>
Device ({record.Name}) has moved outside its assigned geofence.
<br/>
Details:
<br/>
Device Name: [{record.Name}]
<br/>
Last Known Location: [{record.CurrentLocation.X},{record.CurrentLocation.Y}]
<br/>
Timestamp: [{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt")}]
<br/>
Please review this event in the MDM dashboard and take appropriate action if needed.
<br/>
Best regards,
<br/>
Mobcentra – Centralizing Your Mobile World";
            await emailSender.SendAsync("Geofence notification", body, toEmail.SettingValue);
        }

        /// <summary>
        /// Executes configured commands when device moves outside geofence
        /// </summary>
        /// <param name="record">The device record that violated geofence</param>
        /// <param name="geoFencSetting">The geofence settings containing commands to execute</param>
        private async Task HandleGeFencCommandAsync(Device record, GeoFencSetting geoFencSetting, Setting toEmail)
        {
            string[] commands = geoFencSetting.Commands.Split(",") ?? [];

            if (commands.Contains("email"))
            {
                await HandleGeFencEmailAsync(record, toEmail);
            }
            foreach (var item in commands.Where(a => a != "email"))
            {
                try
                {
                    await SendCommandAsync(new SendCommandDto { Command = item.Trim(), Token = [record.Token] });

                }
                catch (Exception)
                {
                    continue;
                }
            }


        }

        /// <summary>
        /// Sends email notification when device battery level drops below warning threshold
        /// Only sends one notification per day per device
        /// </summary>
        /// <param name="record">The device record with battery information</param>
        /// <param name="setting">The setting containing battery warning threshold</param>
        /// <param name="toEmail">The email setting containing recipient address</param>
        private async Task HandleBatteryNotifyAsync(Device record, Setting setting, Setting toEmail)
        {

            if (record.BatteryDate.HasValue && DateTime.UtcNow.Date == record.BatteryDate.Value.Date) return;


            if (!record.BatteryPercentage.IsNullOrEmpty() && Convert.ToInt32(record.BatteryPercentage) <= Convert.ToInt16(setting.SettingValue))
            {
                record.BatteryDate = DateTime.UtcNow;
                string body = $@"
                                         This is an automated alert from the MobCentra
                                         <br/>
                                         The battery level for the device {record.Name} (Device ID: {record.Code}) has dropped to {record.BatteryPercentage}%. Please ensure the device is charged to avoid interruption of service.
                                         <br/>
                                         Details:
                                         <br/>
                                         Device Name: {record.Name}
                                         <br/>
                                         Device ID:{record.Code}
                                         <br/>
                                         Current Battery Level: {record.BatteryPercentage}%
                                         <br/>
                                         Last Sync Time: {DateTime.Now:yyyy-MM-dd hh:mm:ss tt}
                                         <br/>
                                         If you have any questions, please contact the IT support team.
                                         <br/>
                                         Best regards,
                                         <br/>
                                         MobCentra";
                await emailSender.SendAsync("Battery Warning Level", body, toEmail.SettingValue);
            }
        }

        /// <summary>
        /// Processes device queue by sending profile refresh command and removing from queue
        /// </summary>
        /// <param name="record">The device record to process queue for</param>
        private async Task HandleQueuAsync(Device record)
        {
            if (await deviceQueuBll.FindByExpressionAsync(a => a.DeviceId == record.Id) is DeviceQueu deviceQueu)
            {
                try
                {
                    await SendCommandAsync(new SendCommandDto { Command = "refreshProfile", Token = [record.Token] });
                    await deviceQueuBll.DeleteAsync(deviceQueu.Id);
                }
                catch { }
            }
        }

        /// <summary>
        /// Validates that company has not exceeded maximum tracked devices limit
        /// </summary>
        /// <param name="entity">The device entity to check tracking license for</param>
        /// <exception cref="Exception">Thrown if tracking limit is exceeded</exception>
        private async Task CheckTrackedLicenseAsync(Device entity)
        {
            Company currentCompany = await companyBll.Value.FindByExpressionAsync(a => a.Id == entity.CompanyId);

            int currentTrackedCount = (await FindAllByExpressionAsync(a => a.CompanyId == entity.CompanyId && a.TrackActivated == 1)).Count;

            if ((currentTrackedCount + 1) > currentCompany.NoOfTrackedDevices)
            {
                throw new Exception($"لا يمكن اضافة سجل جديد لانك تجازوت الحد الاقصى من السجلات");
            }
        }

        /// <summary>
        /// Calculates the distance between two geographic coordinates using the Haversine formula
        /// </summary>
        /// <param name="lat1">Latitude of first point</param>
        /// <param name="lon1">Longitude of first point</param>
        /// <param name="lat2">Latitude of second point</param>
        /// <param name="lon2">Longitude of second point</param>
        /// <returns>Distance in meters between the two points</returns>
        public static double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // Earth radius in meters
            var latRad1 = Math.PI * lat1 / 180.0;
            var latRad2 = Math.PI * lat2 / 180.0;
            var dLat = Math.PI * (lat2 - lat1) / 180.0;
            var dLon = Math.PI * (lon2 - lon1) / 180.0;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // distance in meters
        }
        /// <summary>
        /// Deletes device log records within the specified date range
        /// </summary>
        /// <param name="fromDate">Start date for deletion range</param>
        /// <param name="toDate">End date for deletion range</param>
        /// <returns>Response indicating success</returns>
        public async Task<DcpResponse<string>> DeleteRecordAsync(DateTime? fromDate, DateTime? toDate)
        {
            Expression<Func<DeviceLog, bool>> expression = x => x.CreatedDate.Date >= fromDate.Value.Date && x.CreatedDate <= toDate.Value.Date;
            var records = await deviceLogBll.FindAllByExpressionAsync(expression);
            await deviceLogBll.DeleteRangeAsync(records);
            return new DcpResponse<string>("", "");
        }
    }
}
