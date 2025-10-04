namespace DcpTracker.Domain.Entities
{
    public class Notifications : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Token { get; set; }
        public Guid CompanyId { get; set; }

    }
}
