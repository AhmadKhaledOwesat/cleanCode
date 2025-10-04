using MobCentra.Application.Bll;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Notification.Bll
{
    public class NotificationBll(IBaseDal<MobCentra.Domain.Entities.Notifications, Guid, NotificationFilter> baseDal,IConstraintBll constraintBll) : BaseBll<MobCentra.Domain.Entities.Notifications, Guid, NotificationFilter>(baseDal), INotificationBll
    {
        public override async Task<PageResult<Domain.Entities.Notifications>> GetAllAsync(NotificationFilter searchParameters) => await base.GetAllAsync(searchParameters);


        public override async Task AddAsync(Domain.Entities.Notifications entity)
        {
            await constraintBll.GetLimitAsync(entity.CompanyId, Domain.Enum.LimitType.NoOfNotifications);
            await base.AddAsync(entity);
        }
    }
}
