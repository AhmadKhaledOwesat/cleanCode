using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class GoogleCommandBll(IBaseDal<GoogleCommand, Guid, GoogleCommandFilter> baseDal) : BaseBll<GoogleCommand, Guid, GoogleCommandFilter>(baseDal), IGoogleCommandBll
    {
        public override Task<PageResult<GoogleCommand>> GetAllAsync(GoogleCommandFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<GoogleCommand, bool>(a => a.Active == 1);
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
