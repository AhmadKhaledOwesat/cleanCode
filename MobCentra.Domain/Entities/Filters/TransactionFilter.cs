namespace MobCentra.Domain.Entities.Filters
{
    public class TransactionFilter : SearchParameters<Transaction>
    {
        public Guid? CompanyId { get; set; }
        public Guid? AppUserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
