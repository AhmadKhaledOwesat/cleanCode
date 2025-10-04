namespace MobCentra.Application.Dto
{
    public class VersionDto : BaseDto<Guid>
    {
        public string VersionName { get; set; }
        public string VersionNumber { get; set; }
        public string VersionLink { get; set; }
    }
}
