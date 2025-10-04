namespace MobCentra.Application.Dto
{
    public class TaskStatusDto : BaseDto<Guid>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public int Active { get; set; }
        public int MobileAppDisplay { get; set; }
        public int BackOfficeDisplay { get; set; }
    }
}
