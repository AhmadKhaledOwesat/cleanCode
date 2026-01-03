using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class GoogleCommandBll(IBaseDal<GoogleCommand, Guid, GoogleCommandFilter> baseDal) : BaseBll<GoogleCommand, Guid, GoogleCommandFilter>(baseDal), IGoogleCommandBll
    {
        public override Task<PageResult<GoogleCommand>> GetAllAsync(GoogleCommandFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<GoogleCommand, bool>(a => a.Active == 1 && (searchParameters.ShowInSetupForm == null || a.ShowInSetupForm == searchParameters.ShowInSetupForm));
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
