using Microsoft.Extensions.Configuration;
using MobCentra.Application.Dto;
using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    public class DeviceBll(IBaseDal<Device, Guid, DeviceFilter> baseDal, INotificationBll notificationBll, IDeviceBatteryTransBll deviceBatteryTransBll, IDeviceTransactionBll deviceTransactionBll, IEmailSender emailSender, IConstraintBll constraintBll, IVersionBll versionBll, Lazy<ICompanyBll> companyBll, Lazy<IGroupBll> groupBll, Lazy<IProfileBll> profileBll, Lazy<IDeviceApplicationBll> deviceApplicationBll, ISettingBll settingBll, IConfiguration configuration, IGeoFencBll geoFencBll, IGeoFencSettingBll geoFencSettingBll, IDeviceLogBll deviceLogBll, ICompanySubscriptionBll companySubscriptionBll, IDeviceQueuBll deviceQueuBll) : BaseBll<Device, Guid, DeviceFilter>(baseDal), IDeviceBll
    {

        public override async Task AddAsync(Device entity)
        {
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfDevices);
            //if (entity.TrackActivated == 1)
            //{
            //    await CheckTrackedLicenseAsync(entity);
            //}
            entity.TrackActivated = 0;
            var record = await base.FindByExpressionAsync(x => x.Code == entity.Code);
            if (record is not null)
            {
                bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(record.CompanyId.Value);
                if (!isValidSubscription)
                    throw new Exception("لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام");

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
                entity.GeoFencDate ??= record.GeoFencDate;
                entity.TrackActivated ??= record.TrackActivated;
                await base.UpdateAsync(entity);
            }
            else
            {
                bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(entity.CompanyId.Value);
                if (!isValidSubscription)
                    throw new Exception("لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام");

                entity.IsOnline = 1;
                await base.AddAsync(entity);
            }
            await SendCommandAsync(new SendCommandDto { Command = "syncDeviceInfo", Token = [entity.Token] });
            await SendCommandAsync(new SendCommandDto { Command = "getStorageInfo", Token = [entity.Token] });

        }

        public async Task<DcpResponse<bool>> UploadImageAndSendCommandAsync(ImageDto imageDto)
        {
            if (imageDto is not null)
            {
                var imagePath = await imageDto.Image.UplodaFiles(".png", "images", Guid.NewGuid().ToString());
                await SendCommandAsync(new SendCommandDto { Command = "changeWallPaper", WallpaperUrl = imagePath, Token = imageDto.Token });
                return new DcpResponse<bool>(true);
            }
            return new DcpResponse<bool>(false, "الرجاء المحاولة لاحقاً");
        }
        public override async Task<PageResult<Device>> GetAllAsync(DeviceFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.StatusId ??= -1;
                searchParameters.PinnedStatusId ??= -1;
                searchParameters.Expression = new Func<Device, bool>(a =>
                    a.CompanyId == searchParameters.CompanyId
                && (searchParameters.Keyword.IsNullOrEmpty() || a.Name.Contains(searchParameters?.Keyword))
                && (searchParameters.GroupId == null || a.GroupId == searchParameters.GroupId)
                && (searchParameters.StatusId == -1 || (a.IsOnline == searchParameters.StatusId))
                && (searchParameters.PinnedStatusId == -1 || ((searchParameters.PinnedStatusId == 1 && !a.UnpinedDate.HasValue) || (searchParameters.PinnedStatusId == 0 && a.UnpinedDate.HasValue)))

                && (searchParameters.ProfileId == null || a.ProfileId == searchParameters.ProfileId)
                && (!searchParameters.FromDate.HasValue || (a.UnpinedDate.HasValue && a.UnpinedDate.Value.Date == searchParameters.FromDate.Value.Date))
                );
            }
            var data = await base.GetAllAsync(searchParameters);
            var setting = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.RefreshTime" && a.CompanyId == searchParameters.CompanyId);

            if (setting != null)
            {
                foreach (var device in data.Collections)
                {
                    if (device.LastSeenDate == null) continue;

                    DateTime lastSeenDate = device.LastSeenDate.Value;

                    int secondsSinceLastSeen = (int)(DateTime.Now - lastSeenDate).TotalSeconds;

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
            return data;
        }
        public async Task<DcpResponse<object>> CheckSettingsAsync(Guid companyId)
        {
            //TODO : use count
            var groupsCount = (await groupBll.Value.GetAllAsync(new GroupFilter { CompanyId = companyId })).Count;
            var devicesCount = (await base.GetAllAsync(new DeviceFilter { CompanyId = companyId })).Count;
            var profilesCount = (await profileBll.Value.GetAllAsync(new ProfileFilter { CompanyId = companyId })).Count;
            var maxCount = (await companyBll.Value.GetByIdAsync(companyId)).NoOfDevices;
            bool isExpired = await companySubscriptionBll.IsValidSubscriptionAsync(companyId);
            return new DcpResponse<object>(new { hasGroups = groupsCount > 0, hasProfiles = profilesCount > 0, hasExeced = devicesCount >= maxCount, isExpired });
        }
        public async Task<DcpResponse<int>> GetVersionCountAsync(Guid companyId)
        {
            //TODO : use count
            var versions = await versionBll.GetAllAsync();
            var lastVersion = versions.LastOrDefault() ?? new Domain.Entities.Version() { VersionNumber = "1.0.0" };
            int allDevices = await base.GetCountByExpressionAsync(x => x.CompanyId == companyId && x.AppVersion != lastVersion.VersionNumber);
            return new DcpResponse<int>(allDevices);
        }
        public async Task<DcpResponse<string>> SendCommandAsync(SendCommandDto sendCommandDto)
        {
            try
            {
                Device device = null;
                if (sendCommandDto.Token.Length > 0)
                {
                    device = await FindByExpressionAsync(x => x.Token == sendCommandDto.Token[0]);

                    if (device == null)
                        return new DcpResponse<string>(null, "الجهاز غير معرف", false);

                    bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(device.CompanyId.Value);
                    if (!isValidSubscription)
                        return new DcpResponse<string>(null, "لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام", false);
                }
                (bool flowControl, DcpResponse<string> value) = await HandleOfflineOnlineStatus(sendCommandDto, device);
                if (!flowControl)
                {
                    return value;
                }

                string path = configuration.GetSection("adminSdkPath").Value;
                var googleCommandSender = new GoogleCommandSender(path, "mdmapp-4bc4a");
                string[] packages = [];
                if (sendCommandDto.Command == "blacklist_settings")
                {
                    packages = (await deviceApplicationBll.Value.FindAllByExpressionAsync(a => a.DeviceId == device.Id && (a.IsBlocked ?? false))).Select(a => a.PackgeName).ToArray();
                }
                if (sendCommandDto.Command == "whitelist_settings")
                {
                    packages = (await deviceApplicationBll.Value.FindAllByExpressionAsync(a => a.DeviceId == device.Id && !(a.IsBlocked ?? false))).Select(a => a.PackgeName).ToArray();
                }

                if (sendCommandDto.Command == "unbind_device")
                {
                    device.UnpinedDate = DateTime.Now;
                    await base.UpdateAsync(device);
                }

                if ((sendCommandDto.GroupId ?? Guid.Empty) != Guid.Empty)
                {
                    var devices = (await FindAllByExpressionAsync(x => x.GroupId == sendCommandDto.GroupId)).Select(a => a.Token).ToArray();
                    sendCommandDto.Token = devices;
                }
                if ((sendCommandDto.CompanyId ?? Guid.Empty) != Guid.Empty)
                {
                    var devices = (await FindAllByExpressionAsync(x => x.CompanyId == sendCommandDto.CompanyId)).Select(a => a.Token).ToArray();
                    sendCommandDto.Token = devices;
                }
                if (sendCommandDto.IsInternal)
                {
                    var setting = (await settingBll.FindByExpressionAsync(x => x.SettingName == "DCP.MDM.URL"));
                    sendCommandDto.ApkUrl = setting.SettingValue;
                }
                foreach (var token in sendCommandDto.Token)
                {
                    try
                    {
                        await googleCommandSender.SendCommandAsync(token, sendCommandDto.Command, packages, sendCommandDto.ApkUrl, sendCommandDto.Password, sendCommandDto.PackageName, sendCommandDto.WallpaperUrl, sendCommandDto.IsInternal, sendCommandDto.FilePath, sendCommandDto.FileName, sendCommandDto.FromDate, sendCommandDto.ToDate);
                        await HandleDeviceLog(sendCommandDto, token, packages);
                    }
                    catch (Exception ex)
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

        private async Task<(bool flowControl, DcpResponse<string> value)> HandleOfflineOnlineStatus(SendCommandDto sendCommandDto, Device device)
        {
            try
            {
                var setting = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.RefreshTime" && a.CompanyId == device.CompanyId);

                if (setting == null) return (true, null);

                if (device.LastSeenDate == null) return (flowControl: false, value: new DcpResponse<string>(null, "الجهاز مغلق حالياُ", false));

                DateTime lastSeenDate = device.LastSeenDate.Value;

                int secondsSinceLastSeen = (int)(DateTime.Now - lastSeenDate).TotalSeconds;
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
        public override async Task UpdateAsync(Device entity)
        {
            var record = await base.FindByExpressionAsync(x => x.Token == entity.Token || entity.Code == x.Code);
            await CheckSubscriptionAsync(record);
            await AddToQueuAsync(entity, record);
            await HandleQueuAsync(record);
            await HandelEmailNofication(entity, record);
            await HandleTracking(entity, record);
            await UpdateDataAsync(entity, record);
        }

        private async Task UpdateDataAsync(Device entity, Device record)
        {
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
            entity.GeoFencDate ??= record.GeoFencDate;
            entity.TrackActivated ??= record.TrackActivated;
            if (!entity.IsFromBackOffice)
            {
                entity.LastSeenDate = DateTime.Now;
            }
            else
            {
                entity.LastSeenDate ??= record.LastSeenDate;
            }
            await base.UpdateAsync(entity);

            await HandleBatteryData(entity, record);

        }

        private async Task HandleBatteryData(Device entity, Device record)
        {
            if (entity.IsFromBackOffice) return;
            DateTime past3Days = DateTime.Now.AddDays(-3);

            var allTrans = await deviceBatteryTransBll.FindAllByExpressionAsync(a => a.TransDateTime <= past3Days);

            if (allTrans != null && allTrans.Count > 0)
            {
                await deviceBatteryTransBll.DeleteRangeAsync(allTrans);
            }
            if (entity.BatteryPercentage != record.BatteryPercentage)
            {
                await deviceBatteryTransBll.AddAsync(new DeviceBatteryTrans { BatteryPercentage = entity.BatteryPercentage, DeviceId = record.Id, TransDateTime = DateTime.Now });
            }
        }

        private async Task AddToQueuAsync(Device entity, Device record)
        {
            if (record.ProfileId != entity.ProfileId && entity.IsFromBackOffice && await deviceQueuBll.FindByExpressionAsync(a => a.DeviceId == record.Id) is null)
            {
                await deviceQueuBll.AddAsync(new DeviceQueu { DeviceId = record.Id });
            }
        }

        private async Task CheckSubscriptionAsync(Device record)
        {
            bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(record.CompanyId.Value);
            if (!isValidSubscription)
                throw new Exception("لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام");
        }

        private async Task<bool> HandleTracking(Device entity, Device record)
        {
            if (record.TrackActivated == 1 && !entity.IsFromBackOffice && false)//ToDO
            {
                var setting = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.TrackingTime" && a.CompanyId == record.CompanyId);
                if (setting is not null)
                {
                    TimeSpan start = DateTime.ParseExact(setting.SettingValue, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                    TimeSpan end = DateTime.ParseExact(setting.SettingValueOt, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                    TimeSpan now = DateTime.Now.TimeOfDay;
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
                deviceTransaction.TransDateTime = DateTime.Now;
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

        private async Task HandleGeoFencNotifyAsync(Device record, Setting toEmail)
        {
            if (record.GeoFencDate.HasValue && DateTime.Now.Date == record.GeoFencDate.Value.Date) return;

            var geoFenc = await geoFencBll.FindByExpressionAsync(a => a.DeviceId == record.Id);
            if (geoFenc != null)
            {
                record.CurrentLocation.SRID = geoFenc.Area.SRID;
                var geoFencSetting = await geoFencSettingBll.FindLastByExpressionAsync(a => a.CompanyId == record.CompanyId);

                bool isInside = geoFenc.Area.Contains(record.CurrentLocation);
                if (!isInside)
                {
                    record.GeoFencDate = DateTime.Now;
                    if (geoFencSetting?.ActionType == Domain.Enum.GeoFencType.Command)
                        await HandleGeFencCommandAsync(record, geoFencSetting);
                    else
                        await HandleGeFencEmailAsync(record, toEmail);
                }
            }
        }

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

        private async Task HandleGeFencCommandAsync(Device record, GeoFencSetting geoFencSetting)
        {
            string[] commands = geoFencSetting.Commands.Split(",") ?? [];
            foreach (var item in commands)
            {
                await SendCommandAsync(new SendCommandDto { Command = item.Trim(), Token = [record.Token] });
            }
        }

        private async Task HandleBatteryNotifyAsync(Device record, Setting setting, Setting toEmail)
        {

            if (record.BatteryDate.HasValue && DateTime.Now.Date == record.BatteryDate.Value.Date) return;


            if (!record.BatteryPercentage.IsNullOrEmpty() && Convert.ToInt32(record.BatteryPercentage) <= Convert.ToInt16(setting.SettingValue))
            {
                record.BatteryDate = DateTime.Now;
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

        private async Task CheckTrackedLicenseAsync(Device entity)
        {
            Company currentCompany = await companyBll.Value.FindByExpressionAsync(a => a.Id == entity.CompanyId);

            int currentTrackedCount = (await FindAllByExpressionAsync(a => a.CompanyId == entity.CompanyId && a.TrackActivated == 1)).Count;

            if ((currentTrackedCount + 1) > currentCompany.NoOfTrackedDevices)
            {
                throw new Exception($"لا يمكن اضافة سجل جديد لانك تجازوت الحد الاقصى من السجلات");
            }
        }

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
        public async Task<DcpResponse<string>> DeleteRecordAsync(DateTime? fromDate, DateTime? toDate)
        {
            Expression<Func<DeviceLog, bool>> expression = x => x.CreatedDate.Date >= fromDate.Value.Date && x.CreatedDate <= toDate.Value.Date;
            var records = await deviceLogBll.FindAllByExpressionAsync(expression);
            await deviceLogBll.DeleteRangeAsync(records);
            return new DcpResponse<string>("", "");
        }
    }
}
