using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface INotificationBll : IBaseBll<Entities.Notifications, Guid, NotificationFilter>
    {
    }
}
