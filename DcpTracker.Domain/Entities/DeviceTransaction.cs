using System.ComponentModel.DataAnnotations.Schema;

namespace DcpTracker.Domain.Entities
{
    public class DeviceTransaction : BaseEntity<Guid>
    {
        public string Coordinations { get; set; }
        public DateTime TransDateTime { get; set; }
        public Guid? DeviceId { get; set; }
        [ForeignKey(nameof(DeviceId))]
        public virtual Device Device { get; set; }
        [NotMapped]
        public string DeviceCode { get; set; }  
        public double? Distance { get; set; }

        [NotMapped]
        public string BatteryPercentage { get; set; }
    }
}
