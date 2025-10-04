using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
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
