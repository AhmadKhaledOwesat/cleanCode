using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MobCentra.Domain.Entities
{
    public class Notifications : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Token { get; set; }
        public Guid? CompanyId { get; set; }

        public Guid? DeviceId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(CreatedBy))]
        public virtual Users CreatedUser { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(DeviceId))]
        public virtual Device Device { get; set; }


    }
}
