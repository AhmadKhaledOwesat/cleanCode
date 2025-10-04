namespace MobCentra.Application.Dto
{
    public class DeviceUsageDto : BaseDto<Guid>
    {
        public Guid DeviceId { get; set; }
        public string AppName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public double? TotalUsage { get; set; }
        public string DeviceCode { get; set; }
    }
}
