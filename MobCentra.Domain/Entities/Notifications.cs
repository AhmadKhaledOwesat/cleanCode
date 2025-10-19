using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MobCentra.Domain.Entities
{
    public class Notifications : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Token { get; set; }
        public Guid CompanyId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(CreatedBy))]
        public virtual Users CreatedUser { get; set; }

    }
}
