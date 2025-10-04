using MobCentra.Domain.Entities.Filters;
using GoogleCommand = MobCentra.Domain.Entities.GoogleCommand;

namespace MobCentra.Domain.Interfaces
{
    public interface IGoogleCommandBll : IBaseBll<GoogleCommand, Guid, GoogleCommandFilter>
    {
    }
}
