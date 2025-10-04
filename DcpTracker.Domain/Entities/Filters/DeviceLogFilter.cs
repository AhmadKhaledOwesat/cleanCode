namespace MobCentra.Domain.Entities.Filters
{
    public class DeviceLogFilter : SearchParameters<DeviceLog>
    {
        public string Description { get; set; }
    }
}
