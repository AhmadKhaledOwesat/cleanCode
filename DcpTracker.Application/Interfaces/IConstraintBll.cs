using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Enum;

namespace MobCentra.Domain.Interfaces
{
    public interface IConstraintBll : IBaseBll<Entities.Application, Guid, ApplicationFilter>
    {
        Task<bool> GetLimitAsync(Guid companyId, LimitType limitType);
    }
}
