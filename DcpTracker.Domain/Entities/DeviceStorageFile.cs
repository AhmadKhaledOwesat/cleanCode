using System.ComponentModel.DataAnnotations.Schema;

namespace DcpTracker.Domain.Entities
{
    public class DeviceStorageFile : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? DeviceId { get; set; }
        [NotMapped]
        public string DeviceCode { get; set; }

        public virtual Device Device { get; set; }

    }
}
