namespace MobCentra.Domain.Entities.Filters
{
    public class ReportFilter : SearchParameters<Report>
    {
        public string Name { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
