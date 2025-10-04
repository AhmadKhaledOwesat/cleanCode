namespace MobCentra.Domain.Entities.Filters
{
    public class DeviceNotificationFilter : SearchParameters<DeviceNotification>
    {
        public string Description { get; set; }
        public int? Active { get; set; }

    }
}
