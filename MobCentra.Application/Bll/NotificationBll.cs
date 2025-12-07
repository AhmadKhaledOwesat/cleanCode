using MobCentra.Application.Bll;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Notification.Bll
{
    /// <summary>
    /// Business logic layer for notification management operations
    /// </summary>
    public class NotificationBll(IBaseDal<MobCentra.Domain.Entities.Notifications, Guid, NotificationFilter> baseDal,IConstraintBll constraintBll) : BaseBll<MobCentra.Domain.Entities.Notifications, Guid, NotificationFilter>(baseDal), INotificationBll
    {
        /// <summary>
        /// Retrieves notifications filtered by company ID
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching notifications</returns>
        public override async Task<PageResult<Domain.Entities.Notifications>> GetAllAsync(NotificationFilter searchParameters)
        {
            // Build search expression filtering by company ID
            searchParameters.Expression = new Func<Domain.Entities.Notifications, bool>(a => a.CompanyId == searchParameters.CompanyId);
           return await base.GetAllAsync(searchParameters);
        }

        /// <summary>
        /// Adds a new notification after validating company notification limits
        /// </summary>
        /// <param name="entity">The notification entity to add</param>
        public override async Task AddAsync(Domain.Entities.Notifications entity)
        {
            // Check if company has reached the maximum number of notifications limit
            await constraintBll.GetLimitAsync(entity.CompanyId, Domain.Enum.LimitType.NoOfNotifications);
            await base.AddAsync(entity);
        }
    }
}
