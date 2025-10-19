using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class TasksBll(IBaseDal<Tasks, Guid, TaskFilter> baseDal,IDeviceBll deviceBll) : BaseBll<Tasks, Guid, TaskFilter>(baseDal), ITasksBll
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
            var tasks = new List<Tasks>();

            if (entity.GroupId.HasValue && entity.GroupId.Value != Guid.Empty)
            {
                var devices = await deviceBll.FindAllByExpressionAsync(a => a.GroupId == entity.GroupId.Value);
                foreach (var item in devices)
                {
                    var newTask = new Tasks
                    {
                        Id = Guid.NewGuid(),        
                        CompanyId = entity.CompanyId,
                        StatusId = entity.StatusId,
                        TaskDescAr = entity.TaskDescAr,
                        DeviceId = item.Id,
                        TaskDescOt = entity.TaskDescOt,
                        TypeId = entity.TypeId,
                        TargetGpsLocation = entity.TargetGpsLocation,
                    };
                    tasks.Add(newTask);
                }
            }

            if (entity.DevicesId?.Length > 0)
            {
                var devices = await deviceBll.FindAllByExpressionAsync(a => entity.DevicesId.Contains(a.Id));
                foreach (var item in devices)
                {
                    var newTask = new Tasks
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = entity.CompanyId,
                        StatusId = entity.StatusId,
                        TaskDescAr = entity.TaskDescAr,
                        DeviceId = item.Id,
                        TaskDescOt = entity.TaskDescOt,
                        TypeId = entity.TypeId,
                        TargetGpsLocation = entity.TargetGpsLocation,
                    };
                    tasks.Add(newTask);
                }
            }

            await AddRangeAsync(tasks);
        }

    }
}
