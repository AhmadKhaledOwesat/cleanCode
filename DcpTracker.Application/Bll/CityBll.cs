using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using DcpTracker.Infrastructure.Extensions;

namespace DcpTracker.Application.Bll
{
    public class CityBll(IBaseDal<City, Guid, CityFilter> baseDal, IConstraintBll constraintBll) : BaseBll<City, Guid, CityFilter>(baseDal), ICityBll
    {
        public override async Task<PageResult<City>> GetAllAsync(CityFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<City, bool>(a =>
                (a.Name == searchParameters?.Description || searchParameters.Description.IsNullOrEmpty()) &&
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
