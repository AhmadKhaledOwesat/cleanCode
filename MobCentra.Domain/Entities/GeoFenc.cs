namespace MobCentra.Domain.Entities
{
    public class GeoFenc : BaseEntity<Guid>
    {
        public Guid DeviceId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? CityId { get; set; }
        public virtual Device Device { get; set; }
        public virtual City City { get; set; }

    }
}
