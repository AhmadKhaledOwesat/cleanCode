using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    public class DeviceTransactionBll(IBaseDal<DeviceTransaction, Guid, DeviceTransactionFilter> baseDal) : BaseBll<DeviceTransaction, Guid, DeviceTransactionFilter>(baseDal), IDeviceTransactionBll
    {

        public override Task<PageResult<DeviceTransaction>> GetAllAsync(DeviceTransactionFilter searchParameters)
        {
            searchParameters.FromDate ??= DateTime.Now;
            searchParameters.ToDate ??= DateTime.Now;
            searchParameters.Expression = new Func<DeviceTransaction, bool>(a => a.DeviceId == searchParameters.DeviceId 
            && ( a.TransDateTime.Date >= searchParameters.FromDate.Value.Date)
            && ( a.TransDateTime.Date <= searchParameters.ToDate.Value.Date));
            return base.GetAllAsync(searchParameters);
        }

        public override async Task AddAsync(DeviceTransaction entity)
        {
            await base.AddAsync(entity);
            //var device = await deviceBll.FindByExpressionAsync(ex => ex.Code == entity.DeviceCode);
            //if (device != null)
            //{

            //    if(device.TrackActivated == 1)
            //    {
            //        entity.DeviceId = device.Id;
            //        entity.Device = null;
            //        var lastTransaction = await base.FindLastByExpressionAsync(x => x.DeviceId == device.Id);
            //        if (lastTransaction != null && !entity.Coordinations.IsNullOrEmpty() && !lastTransaction.Coordinations.IsNullOrEmpty())
            //        {

            //            string[] dbCoordination = lastTransaction.Coordinations.Split(',');
            //            string[] currentCoordination = lastTransaction.Coordinations.Split(',');

            //            entity.Distance = Haversine(
            //                double.Parse(dbCoordination.First().Trim()),
            //                double.Parse(dbCoordination.Last().Trim()),
            //                double.Parse(currentCoordination.First().Trim()),
            //                double.Parse(currentCoordination.Last().Trim()));
            //        }
            //        device.CurrentLocation = entity.Coordinations;
            //        device.TrackActivated = 1;
            //        device.BatteryPercentage = entity.BatteryPercentage;
            //        await deviceBll.UpdateAsync(device);
            //        await base.AddAsync(entity);
            //    }
            //    else
            //    {
            //        device.CurrentLocation = entity.Coordinations;
            //        device.TrackActivated = 0;
            //        device.BatteryPercentage = entity.BatteryPercentage;
            //        await deviceBll.UpdateAsync(device);
            //    }
            //}
        }

        public static double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // Earth radius in meters
            var latRad1 = Math.PI * lat1 / 180.0;
            var latRad2 = Math.PI * lat2 / 180.0;
            var dLat = Math.PI * (lat2 - lat1) / 180.0;
            var dLon = Math.PI * (lon2 - lon1) / 180.0;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // distance in meters
        }
    }
}
