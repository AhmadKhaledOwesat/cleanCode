using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MobCentra.Domain.Entities
{
    public class Tasks : BaseEntity<Guid>
    {
        public Guid? DeviceId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public string TaskDescAr { get; set; }
        public string TaskDescOt { get; set; }
        public string UserNotes { get; set; }
        public DateTime ResponseTime { get; set; }
        public string ResponseGpsLocation { get; set; }
        public string TargetGpsLocation { get; set; }
        public Guid? CompanyId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(CreatedBy))]
        public virtual Users CreatedUser { get; set; }

        [NotMapped]
        public Guid? GroupId { get; set; }
        [NotMapped]
        public Guid[]? DevicesId { get; set; } = [];
    }
}
