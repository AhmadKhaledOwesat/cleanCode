using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using TaskStatus = DcpTracker.Domain.Entities.TaskStatus;

namespace DcpTracker.Application.Bll
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
