namespace MobCentra.Application.Dto
{
    public class UserCommandDto : BaseDto<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid MDMCommandId { get; set; }
        public string MDMCommandName { get; set; }
        public string MDMCommandNameEn { get; set; }

    }
}