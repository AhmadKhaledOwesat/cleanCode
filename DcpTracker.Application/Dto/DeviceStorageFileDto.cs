namespace MobCentra.Application.Dto
{
    public class DeviceStorageFileDto : BaseDto<Guid>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public string DeviceCode { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? DeviceId { get; set; }
    }
}
