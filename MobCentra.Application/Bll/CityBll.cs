using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for city/area management operations
    /// </summary>
    public class CityBll(IBaseDal<City, Guid, CityFilter> baseDal, IConstraintBll constraintBll) : BaseBll<City, Guid, CityFilter>(baseDal), ICityBll
    {
        /// <summary>
        /// Retrieves cities with filtering by keyword (name in Arabic or Other) and company
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching cities</returns>
        public override async Task<PageResult<City>> GetAllAsync(CityFilter searchParameters)
        {
            // Build search expression with keyword and company filters
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<City, bool>(a =>
                    (searchParameters.Keyword.IsNullOrEmpty() || (a.Name.Contains(searchParameters?.Keyword) || a.NameOt.Contains(searchParameters.Keyword))) &&
                a.CompanyId == searchParameters.CompanyId
                );
            }
            return await base.GetAllAsync(searchParameters);
        }

        /// <summary>
        /// Adds a new city/area after validating company area limits
        /// </summary>
        /// <param name="entity">The city entity to add</param>
        public override async Task AddAsync(City entity)
        {
            // Check if company has reached the maximum number of areas limit
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfArea);
            await base.AddAsync(entity);
        }

        public override async Task UpdateAsync(City entity)
        {
            if(entity.RestrictedArea is not null)
            {
                entity.Area = null;
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                var coordinates = new List<Coordinate>();
                var polygons = JsonConvert.DeserializeObject<List<List<CoordinateDto>>>(entity.RestrictedArea);
                foreach (var polygon in polygons)
                    foreach (var point in polygon)
                        coordinates.Add(new Coordinate(point.lng, point.lat));

                if (!coordinates.First().Equals2D(coordinates.Last()))
                {
                    coordinates.Add(coordinates[0]);
                }
                entity.Area = geometryFactory.CreatePolygon([.. coordinates]);
            }
            else
            {
                var dbEntity = await GetByIdAsync(entity.Id);
                entity.Area = dbEntity.Area;
            }
            await base.UpdateAsync(entity);
        }
    }
}
