using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class CompanySubscriptionBll(IBaseDal<CompanySubscription, Guid, CompanySubscriptionFilter> baseDal) : BaseBll<CompanySubscription, Guid, CompanySubscriptionFilter>(baseDal), ICompanySubscriptionBll
    {

        public async Task<bool> IsValidSubscriptionAsync(Guid companyId)
        {
            List<CompanySubscription> companySubscriptions = await FindAllByExpressionAsync(x => x.CompanyId == companyId);
            if (companySubscriptions is { Count: > 0 })
            {
                CompanySubscription companySubscription = companySubscriptions.LastOrDefault();
                return new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) < companySubscription.ToDate;
            }
            return false;
        }
    }
}
