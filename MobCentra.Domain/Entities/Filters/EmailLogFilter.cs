namespace MobCentra.Domain.Entities.Filters
{
    public class EmailLogFilter : SearchParameters<EmailLog>
    {
        public Guid? CompanyId { get; set; }
        public string SendStatus { get; set; }
    }
}
