using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using DcpTracker.Infrastructure.Extensions;

namespace DcpTracker.Application.Bll
{
    public class BlackListAppBll(IBaseDal<BlackListApp, Guid, BlackListAppFilter> baseDal) : BaseBll<BlackListApp, Guid, BlackListAppFilter>(baseDal), IBlackListAppBll
    {
        public override Task<PageResult<BlackListApp>> GetAllAsync(BlackListAppFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<BlackListApp, bool>(a => 
                    
                    (a.Name == searchParameters?.Description || searchParameters.Description.IsNullOrEmpty())
                    && a.CompanyId == searchParameters.CompanyId

                    );
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
