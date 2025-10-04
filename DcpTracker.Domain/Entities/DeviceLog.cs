using System.ComponentModel.DataAnnotations.Schema;

namespace DcpTracker.Domain.Entities
{
    public class DeviceLog : BaseEntity<Guid>
    {
        public Guid? DeviceId { get; set; }
        [ForeignKey(nameof(DeviceId))]
        public virtual Device Device { get; set; }
        public string CommandName { get; set; }
        public string Data { get; set; }
    }
}
