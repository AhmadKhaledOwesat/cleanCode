using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MobCentra.Domain.Entities
{
    public class DeviceLog : BaseEntity<Guid>
    {
        public Guid? DeviceId { get; set; }
        [ForeignKey(nameof(DeviceId))]
        public virtual Device Device { get; set; }
        public string CommandName { get; set; }
        public string Data { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(CreatedBy))]
        public virtual Users CreatedUser { get; set; }
    }
}
