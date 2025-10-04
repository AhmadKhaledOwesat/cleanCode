using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using CommandGroup = MobCentra.Domain.Entities.CommandGroup;

namespace MobCentra.Application.Bll
{
    public class CommandGroupBll(IBaseDal<CommandGroup, Guid, CommandGroupFilter> baseDal) : BaseBll<CommandGroup, Guid, CommandGroupFilter>(baseDal), ICommandGroupBll
    {
        public override Task<PageResult<CommandGroup>> GetAllAsync(CommandGroupFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<CommandGroup, bool>(a => 
                    
                    (a.NameAr == searchParameters?.Description || searchParameters.Description.IsNullOrEmpty()) 
                    && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
