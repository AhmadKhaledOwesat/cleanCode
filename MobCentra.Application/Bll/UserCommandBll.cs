using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class UserCommandBll(IBaseDal<UserCommand, Guid, UserCommandFilter> baseDal) : BaseBll<UserCommand, Guid, UserCommandFilter>(baseDal), IUserCommandBll
    {
        public override Task<PageResult<UserCommand>> GetAllAsync(UserCommandFilter searchParameters)
        {
            searchParameters.Expression = new Func<UserCommand, bool>(a => a.UserId == searchParameters.UserId);
            return base.GetAllAsync(searchParameters);
        }
    }
}
