using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using TaskStatus = DcpTracker.Domain.Entities.TaskStatus;

namespace DcpTracker.Domain.Interfaces
{
    public interface ITaskStatusBll : IBaseBll<TaskStatus, Guid, TaskStatusFilter>
    {
    }
}
