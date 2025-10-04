using System.ComponentModel.DataAnnotations.Schema;

namespace DcpTracker.Domain.Entities
{
    public class DeviceUsage : BaseEntity<Guid>
    {
        public Guid DeviceId { get; set; }
        public string AppName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public double? TotalUsage { get; set; }

        [NotMapped]
        public string DeviceCode { get; set; }
    }
}
