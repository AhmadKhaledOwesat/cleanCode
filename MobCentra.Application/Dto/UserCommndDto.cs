namespace MobCentra.Application.Dto
{
    public class UserCommandDto : BaseDto<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid GoogleCommandId { get; set; }
        public string GoogleCommandName { get; set; }
    }
}