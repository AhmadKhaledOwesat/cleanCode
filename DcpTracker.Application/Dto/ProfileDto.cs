﻿
namespace MobCentra.Application.Dto
{
    public class ProfileDto : BaseDto<Guid>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public int Active { get; set; }
        public Guid? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public virtual ICollection<ProfileFeatureDto> ProfileFeatures { get; set; }

    }
}
