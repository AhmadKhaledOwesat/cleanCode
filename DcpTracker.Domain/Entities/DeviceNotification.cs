using MobCentra.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
{
    public class DeviceNotification : BaseEntity<Guid>
    {
        public DeviceOperationType TypeId { get; set; }
        public string Location { get; set; }
        public string BatteryPercentage { get; set; }   
        public Guid? DeviceId { get; set; }
        [ForeignKey(nameof(DeviceId))]
        public virtual Device Device { get; set; }

        [NotMapped]
        public string DeviceCode { get; set; }
        public string ScreenShot { get; set; }

    }
}
