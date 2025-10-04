using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class TasksBll(IBaseDal<Tasks, Guid, TaskFilter> baseDal) : BaseBll<Tasks, Guid, TaskFilter>(baseDal), ITasksBll
    {
        public override Task<PageResult<Tasks>> GetAllAsync(TaskFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Tasks, bool>(a =>
               (searchParameters.DeviceId == null || a.DeviceId == searchParameters.DeviceId) &&
               (!searchParameters.FromDate.HasValue || searchParameters.FromDate.Value.Date >= a.CreatedDate.Date) &&
               (!searchParameters.ToDate.HasValue || searchParameters.ToDate.Value.Date <= a.CreatedDate.Date));
            }

            return base.GetAllAsync(searchParameters);
        }

        public override async Task AddAsync(Tasks entity)
        {
           
                await base.AddAsync(entity);
        }

    }
}
