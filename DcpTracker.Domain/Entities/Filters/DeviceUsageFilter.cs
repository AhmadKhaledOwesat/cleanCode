namespace DcpTracker.Domain.Entities.Filters
{
    public class DeviceUsageFilter : SearchParameters<DeviceUsage>
    {
        public Guid? DeviceId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string AppName { get; set; }
    }
}
