namespace DcpTracker.Application.Dto
{
    public class DeviceBatteryTransDto : BaseDto<Guid>
    {
        public DateTime TransDateTime { get; set; }
        public string BatteryPercentage { get; set; }
        public Guid? DeviceId { get; set; }
    }
}
