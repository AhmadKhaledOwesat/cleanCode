using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class MDMCommandBll(IBaseDal<MDMCommand, Guid, MDMCommandFilter> baseDal) : BaseBll<MDMCommand, Guid, MDMCommandFilter>(baseDal), IMDMCommandBll
    {
        public override Task<PageResult<MDMCommand>> GetAllAsync(MDMCommandFilter searchParameters)
        {
            searchParameters.Expression = new Func<MDMCommand, bool>(a => a.Active == 1 && (searchParameters.ShowInSetupForm == null || a.ShowInSetupForm == searchParameters.ShowInSetupForm));
            return base.GetAllAsync(searchParameters);
        }
    }
}
