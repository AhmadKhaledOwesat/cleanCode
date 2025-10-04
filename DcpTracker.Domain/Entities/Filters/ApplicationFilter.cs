namespace DcpTracker.Domain.Entities.Filters
{
    public class ApplicationFilter : SearchParameters<Application>
    {
        public string Description { get; set; }
        public int? Active { get; set; }
        public Guid? CompanyId { get; set; }    

    }
}
