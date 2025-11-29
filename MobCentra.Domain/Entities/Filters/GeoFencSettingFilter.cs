namespace MobCentra.Domain.Entities.Filters
{
    public class GeoFencSettingFilter : SearchParameters<GeoFencSetting>
    {
        public Guid? CompanyId { get; set; }    

    }
}
