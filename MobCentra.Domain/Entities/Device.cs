using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
{
    public class Device : BaseEntity<Guid>
    {
        public string AppVersion { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Token { get; set; }
        public string TotalSpace { get; set; }
        public string FreeSpace { get; set; }
        public string UsedSpace { get; set; }
        public string BatteryPercentage { get; set; }
        public Point? CurrentLocation { get; set; }
        public int? IsOnline { get; set; }
        public string OSVersion { get; set; }
        public string IMEI { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string BatteryCapacity { get; set; }
        public string ScreenSize { get; set; }
        public Guid? CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }

        public Guid? GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }
        public virtual ICollection<DeviceNotification> DeviceNotifications { get; set; }
        public virtual ICollection<DeviceApplication> DeviceApplications { get; set; }
        public virtual ICollection<DeviceLog> DeviceLogs { get; set; }
        public virtual ICollection<DeviceTransaction> DeviceTransactions { get; set; }
        public virtual GeoFenc GeoFenc { get; set; }
        public virtual ICollection<DeviceFile> DeviceFiles { get; set; }
        public virtual ICollection<DeviceStorageFile> DeviceStorageFiles { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
        public string ImagesSpace { get; set; }
        public string VideosSpace { get; set; }
        public string AudioSpace { get; set; }
        public string DocumentsSpace { get; set; }
        public string OtherSpace { get; set; }
        public string SystemSpace { get; set; }
        public DateTime? UnpinedDate { get; set; }
        public Guid? ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public DateTime? LastSeenDate { get; set; }
        public int? TrackActivated { get; set; }
        [NotMapped]
        public bool IsFromBackOffice { get; set; }=false;

        public DateTime? GeoFencDate { get; set; }
        public DateTime? BatteryDate { get; set; }
        public DateTime? DeviceDateTime { get; set; }
        [NotMapped]
        public bool? IsWrongTime { get; set; }



    }
}
