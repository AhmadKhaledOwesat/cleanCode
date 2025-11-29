using MobCentra.Domain.Enum;

namespace MobCentra.Domain.Entities
{
    public class GeoFencSetting : BaseEntity<Guid>
    {
        public Guid CompanyId { get; set; }
        public GeoFencType ActionType { get; set; }
        public string Commands { get; set; }
    }
}
