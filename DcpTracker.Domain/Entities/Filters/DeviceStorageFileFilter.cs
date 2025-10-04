namespace DcpTracker.Domain.Entities.Filters
{
    public class DeviceStorageFileFilter : SearchParameters<DeviceStorageFile>
    {
        public string Description { get; set; }
        public int? Active { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? DeviceId { get; set; }


    }
}
