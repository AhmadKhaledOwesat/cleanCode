using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using TaskStatus = MobCentra.Domain.Entities.TaskStatus;

namespace MobCentra.Domain.Interfaces
{
    public interface ITaskStatusBll : IBaseBll<TaskStatus, Guid, TaskStatusFilter>
    {
    }
}
