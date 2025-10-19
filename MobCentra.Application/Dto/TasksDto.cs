namespace MobCentra.Application.Dto
{
    public class TasksDto : BaseDto<Guid>
    {
        public Guid? GroupId { get; set; }
        public Guid[]? DevicesId { get; set; } = [];
        public Guid? DeviceId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public string TaskDescAr { get; set; }
        public string TaskDescOt { get; set; }
        public string UserNotes { get; set; }
        public DateTime ResponseTime { get; set; }
        public string ResponseGpsLocation { get; set; }
        public string TargetGpsLocation { get; set; }   
        public Guid? CompanyId { get; set; }
        public string CreatedByName { get; set; }

    }
}
