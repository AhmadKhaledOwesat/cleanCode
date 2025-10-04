using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using DcpTracker.Infrastructure.Extensions;
using CommandGroup = DcpTracker.Domain.Entities.CommandGroup;

namespace DcpTracker.Application.Bll
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
