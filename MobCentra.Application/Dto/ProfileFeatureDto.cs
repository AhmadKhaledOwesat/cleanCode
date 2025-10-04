namespace MobCentra.Application.Dto
{
    public class ProfileFeatureDto : BaseDto<Guid>
    {
        public Guid ProfileId { get; set; }
        public string ProfileName { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
    }
}
