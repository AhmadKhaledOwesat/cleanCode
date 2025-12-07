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
                item.Result = await baseDal.ExecuteSqlAsync(item.Query);
                item.ResultOt = await baseDal.ExecuteSqlAsync(item.QueryOt);
            }
            data.Collections = [.. data.Collections.OrderBy(a => a.SortOrder)];
            return data;
        }
    }
}
