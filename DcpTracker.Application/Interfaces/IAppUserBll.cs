using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IAppUserBll : IBaseBll<AppUsers, Guid, AppUserFilter>
    {
        Task<DcpResponse<AppUserDto>> LoginAsync(LoginDto loginDto);
    }
}
