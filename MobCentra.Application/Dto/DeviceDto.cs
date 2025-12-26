
using MobCentra.Domain.Entities;

namespace MobCentra.Application.Dto
{
    public class DeviceDto : BaseDto<Guid>
    {
        public string AppVersion { get; set; }
        public string ImagesSpace { get; set; }
        public string VideosSpace { get; set; }
        public string AudioSpace { get; set; }
        public string DocumentsSpace { get; set; }
        public string OtherSpace { get; set; }
        public string SystemSpace { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? UnpinedDate { get; set; }

        public string Token { get; set; }
        public string TotalSpace { get; set; }
        public string FreeSpace { get; set; }
        public string UsedSpace { get; set; }
        public string BatteryPercentage { get; set; }
        public string CurrentLocation { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileNameOt { get; set; }

        public string GroupName { get; set; }
       public string GroupNameOt { get; set; }
        public string CompanyName { get; set; }
        public string OSVersion { get; set; }
        public string IMEI { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string BatteryCapacity { get; set; }
        public string ScreenSize { get; set; }
        public int? IsOnline {  get; set; }
        public DateTime? LastSeenDate { get; set; }
        public virtual ICollection<DeviceNotificationDto> DeviceNotifications { get; set; }
        public virtual ICollection<DeviceApplicationDto> DeviceApplications { get; set; }
        public virtual ICollection<DeviceLogDto> DeviceLogs { get; set; }
        public virtual ICollection<DeviceTransactionDto> DeviceTransactions { get; set; }
        public virtual ICollection<DeviceFileDto> DeviceFiles { get; set; }
        public virtual ICollection<DeviceStorageFileDto> DeviceStorageFiles { get; set; }
        public virtual ICollection<TasksDto> Tasks { get; set; }

        public bool IsFromBackOffice { get; set; } = false;
        public int? TrackActivated { get; set; }
        public virtual GeoFencDto GeoFenc { get; set; }
        public DateTime? GeoFencDate { get; set; }
        public DateTime? BatteryDate { get; set; }
        public DateTime? DeviceDateTime { get; set; }
        public bool? IsWrongTime { get; set; }

    }
}
