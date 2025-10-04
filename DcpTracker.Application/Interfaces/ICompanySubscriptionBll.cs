using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface ICompanySubscriptionBll : IBaseBll<CompanySubscription, Guid, CompanySubscriptionFilter>
    {
        Task<bool> IsValidSubscriptionAsync(Guid companyId);
    }
}
