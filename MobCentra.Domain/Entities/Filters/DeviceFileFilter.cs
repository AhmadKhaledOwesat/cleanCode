namespace MobCentra.Domain.Entities.Filters
{
    public class DeviceFileFilter : SearchParameters<DeviceFile>
    {
        public string Description { get; set; }
        public int? Active { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? DeviceId { get; set; }


    }
}
