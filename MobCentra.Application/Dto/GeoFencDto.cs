namespace MobCentra.Application.Dto
{
    public class GeoFencDto : BaseDto<Guid>
    {
        public Guid DeviceId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CityId { get; set; }
    }
}
