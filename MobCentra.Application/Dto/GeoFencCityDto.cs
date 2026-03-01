
namespace MobCentra.Application.Dto
{
    public class GeoFencCityDto : BaseDto<Guid>
    {
        public Guid ComapnyId { get; set; }
        public Guid DeviceId { get; set; }
        public Guid CityId { get; set; }
    }
}
