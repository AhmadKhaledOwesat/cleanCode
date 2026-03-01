using MobCentra.Domain.Entities.Filters;
using MDMCommand = MobCentra.Domain.Entities.MDMCommand;

namespace MobCentra.Domain.Interfaces
{
    public interface IMDMCommandBll : IBaseBll<MDMCommand, Guid, MDMCommandFilter>
    {
    }
}
