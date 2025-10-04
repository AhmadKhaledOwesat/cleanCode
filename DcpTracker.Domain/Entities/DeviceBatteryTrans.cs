namespace DcpTracker.Domain.Entities
{
    public class DeviceBatteryTrans : BaseEntity<Guid>
    {
        public DateTime TransDateTime { get; set; }
        public string BatteryPercentage { get; set; }
        public Guid? DeviceId { get; set; }
    }
}
