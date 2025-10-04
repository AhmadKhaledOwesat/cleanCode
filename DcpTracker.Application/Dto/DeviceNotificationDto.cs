
using DcpTracker.Domain.Enum;

namespace DcpTracker.Application.Dto
{
    public class DeviceNotificationDto : BaseDto<Guid>
    {
        public DeviceOperationType TypeId { get; set; }
        public string Location { get; set; }
        public Guid? DeviceId { get; set; }
        public string BatteryPercentage { get; set; }
        public string  DeviceName { get; set; }
        public string DeviceCode { get; set; }
        public string ScreenShot { get; set; }  

    }
}
