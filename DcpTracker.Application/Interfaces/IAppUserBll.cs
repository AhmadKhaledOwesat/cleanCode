using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IAppUserBll : IBaseBll<AppUsers, Guid, AppUserFilter>
    {
        Task<DcpResponse<AppUserDto>> LoginAsync(LoginDto loginDto);
    }
}
