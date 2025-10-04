namespace MobCentra.Domain.Entities.Filters
{
    public class DeviceBatteryTransFilter : SearchParameters<DeviceBatteryTrans>
    {
        public Guid? DeviceId { get; set; }

    }
}
