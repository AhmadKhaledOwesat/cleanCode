using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
{
    public class DeviceApplication : BaseEntity<Guid>
    {
        public Guid? DeviceId { get; set; }
        [ForeignKey(nameof(DeviceId))]
        public virtual Device Device { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Size { get; set; }
        public string PackgeName { get; set; }
        public bool IsSystem { get; set; }
        [NotMapped]
        public string Code { get; set; }
        public bool? IsBlocked {  get; set; }

    }
}
