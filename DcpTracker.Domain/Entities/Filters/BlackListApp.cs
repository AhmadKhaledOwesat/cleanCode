namespace DcpTracker.Domain.Entities.Filters
{
    public class BlackListAppFilter : SearchParameters<BlackListApp>
    {
        public string Description { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
