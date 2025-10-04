using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Enum;

namespace DcpTracker.Domain.Interfaces
{
    public interface IConstraintBll : IBaseBll<Entities.Application, Guid, ApplicationFilter>
    {
        Task<bool> GetLimitAsync(Guid companyId, LimitType limitType);
    }
}
