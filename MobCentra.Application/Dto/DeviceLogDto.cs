namespace MobCentra.Application.Dto
{
    public class DeviceLogDto : BaseDto<Guid>
    {
        public string CommandName { get; set; }
        public string Data { get; set; }
        public Guid? DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string CreatedByName { get; set; }

    }
}
