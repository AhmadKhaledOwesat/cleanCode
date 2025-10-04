namespace DcpTracker.Application.Dto
{
    public class DeviceApplicationDto : BaseDto<Guid>
    {
        public Guid? DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Size { get; set; }
        public string PackgeName { get; set; }
        public string Code { get; set; }
        public bool IsSystem { get; set; }
        public bool? IsBlocked { get; set; }


    }
    public class DeviceBlockedApplicationDto : BaseDto<Guid>
    {
        public bool? IsBlocked { get; set; }
    }
}
