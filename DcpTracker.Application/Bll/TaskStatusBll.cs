using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using TaskStatus = MobCentra.Domain.Entities.TaskStatus;

namespace MobCentra.Application.Bll
{
    public class TaskStatusBll(IBaseDal<TaskStatus, Guid, TaskStatusFilter> baseDal) : BaseBll<TaskStatus, Guid, TaskStatusFilter>(baseDal), ITaskStatusBll
    {
        public override Task<PageResult<TaskStatus>> GetAllAsync(TaskStatusFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Description))
                    searchParameters.Expression = new Func<TaskStatus, bool>(a => a.NameAr == searchParameters?.Description && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
