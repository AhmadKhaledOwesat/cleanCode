namespace MobCentra.Domain.Entities.Filters
{
    public class TaskFilter : SearchParameters<Tasks>
    {
        public string TaskName { get; set; }
        public int? StatusId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? DeviceId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
