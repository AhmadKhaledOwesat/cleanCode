namespace MobCentra.Application.Dto
{
    public class UserGroupDto : BaseDto<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
    }
}