namespace DcpTracker.Domain.Entities.Filters
{
    public class NotificationFilter : SearchParameters<Notifications>
    {
        public Guid? CompanyId { get; set; }    
    }
}
