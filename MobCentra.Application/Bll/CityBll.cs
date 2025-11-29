using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    public class CityBll(IBaseDal<City, Guid, CityFilter> baseDal, IConstraintBll constraintBll) : BaseBll<City, Guid, CityFilter>(baseDal), ICityBll
    {
        public override async Task<PageResult<City>> GetAllAsync(CityFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<City, bool>(a =>
                    (searchParameters.Keyword.IsNullOrEmpty() || a.Name.Contains(searchParameters?.Keyword)) &&
                a.CompanyId == searchParameters.CompanyId
                );
            }
            return await base.GetAllAsync(searchParameters);
        }

        public override async Task AddAsync(City entity)
        {
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfArea);
            await base.AddAsync(entity);
        }
    }
}
