
namespace MobCentra.Application.Dto
{
    public class DeviceTransactionDto : BaseDto<Guid>
    {
        public string Coordinations { get; set; }
        public DateTime TransDateTime { get; set; }
        public Guid? DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceCode { get; set; }
        public double? Distance { get; set; }
        public string BatteryPercentage { get; set; }


    }
}
