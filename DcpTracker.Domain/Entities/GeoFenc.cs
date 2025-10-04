using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
{
    public class GeoFenc : BaseEntity<Guid>
    {
        public Guid DeviceId { get; set; }
        public Guid CompanyId { get; set; }
        public Polygon Area { get; set; }
        [NotMapped]
        public string RestrictedArea { get; set; }

        public virtual Device Device { get; set; }   

    }
}
