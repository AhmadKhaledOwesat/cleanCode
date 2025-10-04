namespace DcpTracker.Domain.Entities
{
    public class Report : BaseEntity<Guid>
    {
        public string ReportName { get; set; }
        public string ReportProcedure { get; set; }
        public Guid? CompanyId { get; set; }
        public virtual ICollection<ReportParameter> ReportParameters { get; set; } = [];
    }
}
