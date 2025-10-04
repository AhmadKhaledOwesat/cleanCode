namespace MobCentra.Domain.Entities.Filters
{
    public class TaskStatusFilter : SearchParameters<TaskStatus>
    {
        public string Description { get; set; }
        public int? Active { get; set; }

    }
}
