namespace DcpTracker.Domain.Entities.Filters
{
    public class DeviceApplicationFilter : SearchParameters<DeviceApplication>
    {
        public string Description { get; set; }
        public int? Active { get; set; }

        public Guid? DeviceId { get; set; }

    }
}
