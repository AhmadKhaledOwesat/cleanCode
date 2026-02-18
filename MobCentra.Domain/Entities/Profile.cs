using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
{
    public class Profile : BaseEntity<Guid>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public int Active { get; set; }
        public Guid? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }

        public virtual ICollection<ProfileFeature> ProfileFeatures { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual Users User { get; set; }
    }
}
