namespace DcpTracker.Domain.Entities
{
    public class CompanySubscription : BaseEntity<Guid>
    {
        public Guid? CompanyId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
}
