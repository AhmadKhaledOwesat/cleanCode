using DcpTracker.Application.Bll;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Notification.Bll
{
    public class NotificationBll(IBaseDal<DcpTracker.Domain.Entities.Notifications, Guid, NotificationFilter> baseDal,IConstraintBll constraintBll) : BaseBll<DcpTracker.Domain.Entities.Notifications, Guid, NotificationFilter>(baseDal), INotificationBll
    {
        public override async Task<PageResult<Domain.Entities.Notifications>> GetAllAsync(NotificationFilter searchParameters) => await base.GetAllAsync(searchParameters);


        public override async Task AddAsync(Domain.Entities.Notifications entity)
        {
            await constraintBll.GetLimitAsync(entity.CompanyId, Domain.Enum.LimitType.NoOfNotifications);
            await base.AddAsync(entity);
        }
    }
}
