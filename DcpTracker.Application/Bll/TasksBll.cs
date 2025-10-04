using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
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
