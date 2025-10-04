using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class UserCommandBll(IBaseDal<UserCommand, Guid, UserCommandFilter> baseDal) : BaseBll<UserCommand, Guid, UserCommandFilter>(baseDal), IUserCommandBll
    {
        public override Task<PageResult<UserCommand>> GetAllAsync(UserCommandFilter searchParameters)
        {
            return base.GetAllAsync(searchParameters);
        }
    }
}
