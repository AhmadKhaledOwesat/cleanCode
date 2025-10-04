namespace DcpTracker.Domain.Entities.Filters
{
    public class SettingFilter : SearchParameters<Setting>
    {
        public string Term { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
