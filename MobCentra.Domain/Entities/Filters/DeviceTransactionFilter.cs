namespace MobCentra.Domain.Entities.Filters
{
    public class DeviceTransactionFilter : SearchParameters<DeviceTransaction>
    {
        public Guid? CompanyId { get; set; }
        public Guid? DeviceId { get; set; }
        public Guid? AppUserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
