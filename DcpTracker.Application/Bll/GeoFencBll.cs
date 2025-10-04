using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using static Grpc.Core.Metadata;

namespace DcpTracker.Application.Bll
{
    public class GeoFencBll(IBaseDal<GeoFenc, Guid, GeoFencFilter> baseDal) : BaseBll<GeoFenc, Guid, GeoFencFilter>(baseDal), IGeoFencBll
    {
        public override async Task AddAsync(GeoFenc entity)
        {

            var record = await FindByExpressionAsync(a => a.DeviceId == entity.DeviceId);
            if (record is not null)
                await DeleteAsync(record.Id);

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
            await base.AddAsync(entity);
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var record = await FindByExpressionAsync(a => a.DeviceId == id);
            if (record is not null)
                await base.DeleteAsync(record.Id);

            return await Task.FromResult(true);
        }
    }
}
