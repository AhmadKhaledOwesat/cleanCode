namespace MobCentra.Application.Dto
{
    public class EmailLogDto : BaseDto<Guid>
    {
        public string Function { get; set; } = string.Empty;
        public string ReceivedEmail { get; set; } = string.Empty;
        public string SendStatus { get; set; } = string.Empty;
        public Guid? DeviceId { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
