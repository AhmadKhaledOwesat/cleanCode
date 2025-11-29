using MobCentra.Domain.Enum;

namespace MobCentra.Application.Dto
{
    public class GeoFencSettingDto : BaseDto<Guid>
    {
        public Guid CompanyId { get; set; }
        public GeoFencType ActionType { get; set; }
        public string Commands { get; set; }

    }
}
