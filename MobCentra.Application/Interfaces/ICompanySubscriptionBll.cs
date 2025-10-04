using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface ICompanySubscriptionBll : IBaseBll<CompanySubscription, Guid, CompanySubscriptionFilter>
    {
        Task<bool> IsValidSubscriptionAsync(Guid companyId);
    }
}
