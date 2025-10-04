using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class StatisticBll(IBaseDal<Statistic, Guid, StatisticFilter> baseDal) : BaseBll<Statistic, Guid, StatisticFilter>(baseDal), IStatisticBll
    {
        public override async Task<PageResult<Statistic>> GetAllAsync(StatisticFilter searchParameters)
        {

            searchParameters.Expression = new Func<Statistic, bool>(a => a.CompanyId == searchParameters.CompanyId && a.Active == 1);

            var data = await base.GetAllAsync(searchParameters);

            foreach (var item in data.Collections)
            {
                item.Result = await baseDal.ExecuteSQL(item.Query);
                item.ResultOt = await baseDal.ExecuteSQL(item.QueryOt);
            }
            data.Collections = data.Collections.OrderBy(a => a.SortOrder).ToList();
            return data;
        }
    }
}
