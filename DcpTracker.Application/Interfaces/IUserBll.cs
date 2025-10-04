using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Interfaces
{
    public interface IUserBll : IBaseBll<Users, Guid, UserFilter>
    {
        Task<DcpResponse<UsersDto>> LoginAsync(string userName, string password, string companyCode);
        Task<DcpResponse<string>> ResetPasswordAsync(string userName, string companyCode);
        Task<DcpResponse<string>> UpdatePasswordAsync(Guid userId, string newPassword);
    }
}
