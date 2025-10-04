using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Interfaces
{
    public interface IUserBll : IBaseBll<Users, Guid, UserFilter>
    {
        Task<DcpResponse<UsersDto>> LoginAsync(string userName, string password, string companyCode);
        Task<DcpResponse<string>> ResetPasswordAsync(string userName, string companyCode);
        Task<DcpResponse<string>> UpdatePasswordAsync(Guid userId, string newPassword);
    }
}
