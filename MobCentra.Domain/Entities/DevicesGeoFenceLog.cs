using NetTopologySuite.Geometries;

namespace MobCentra.Domain.Entities
{
    public class DevicesGeoFenceLog : BaseEntity<Guid>
    {
        public int TransType { get; set; }
        public DateTime TransDate { get; set; }
        public Point Coordinations { get; set; }
        public Guid? DeviceId { get; set; }
        public virtual Device Device { get; set; }
    }
}
