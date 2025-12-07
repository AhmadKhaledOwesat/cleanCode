using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for company subscription management operations
    /// </summary>
    public class CompanySubscriptionBll(IBaseDal<CompanySubscription, Guid, CompanySubscriptionFilter> baseDal) : BaseBll<CompanySubscription, Guid, CompanySubscriptionFilter>(baseDal), ICompanySubscriptionBll
    {
        /// <summary>
        /// Checks if a company has a valid active subscription
        /// </summary>
        /// <param name="companyId">The company identifier to check subscription for</param>
        /// <returns>True if company has an active subscription (end date is in the future), false otherwise</returns>
        public async Task<bool> IsValidSubscriptionAsync(Guid companyId)
        {
            // Get all subscriptions for the company
            List<CompanySubscription> companySubscriptions = await FindAllByExpressionAsync(x => x.CompanyId == companyId);
            if (companySubscriptions is { Count: > 0 })
            {
                // Check if the latest subscription is still valid (end date is in the future)
                CompanySubscription companySubscription = companySubscriptions.LastOrDefault();
                return new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) < companySubscription.ToDate;
            }
            return false;
        }
    }
}
