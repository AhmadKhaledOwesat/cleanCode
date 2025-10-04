namespace MobCentra.Application.Dto
{
    public class CommandGroupDto : BaseDto<Guid>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public int Active { get; set; }
    }
}
