using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
{
    public class City : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string NameOt { get; set; }
        public string Location { get; set; }
        public Guid? CompanyId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual Users User { get; set; }
        public Polygon Area { get; set; }
        [NotMapped]
        public string RestrictedArea { get; set; }
    }
}
