namespace MobCentra.Application.Dto
{
    public class DevicesGeoFenceLogDto : BaseDto<Guid>
    {
        public int TransType { get; set; }
        public DateTime TransDate { get; set; }
        public string Coordinations { get; set; }
        public Guid? DeviceId { get; set; }
    }
}
