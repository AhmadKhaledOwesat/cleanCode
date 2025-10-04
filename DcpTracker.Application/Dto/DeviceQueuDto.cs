namespace DcpTracker.Application.Dto
{
    public class DeviceQueuDto : BaseDto<Guid>
    {
        public Guid? DeviceId { get; set; }
    }
}
