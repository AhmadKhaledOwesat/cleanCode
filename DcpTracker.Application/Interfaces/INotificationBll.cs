using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface INotificationBll : IBaseBll<Entities.Notifications, Guid, NotificationFilter>
    {
    }
}
