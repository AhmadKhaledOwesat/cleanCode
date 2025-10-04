namespace MobCentra.Domain.Entities.Filters
{
    public class GeoFencFilter : SearchParameters<GeoFenc>
    {
        public Guid? DeviceId { get; set; }
        public Guid? CompanyId { get; set; }    

    }
}
