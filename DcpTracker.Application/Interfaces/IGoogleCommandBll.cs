using DcpTracker.Domain.Entities.Filters;
using GoogleCommand = DcpTracker.Domain.Entities.GoogleCommand;

namespace DcpTracker.Domain.Interfaces
{
    public interface IGoogleCommandBll : IBaseBll<GoogleCommand, Guid, GoogleCommandFilter>
    {
    }
}
