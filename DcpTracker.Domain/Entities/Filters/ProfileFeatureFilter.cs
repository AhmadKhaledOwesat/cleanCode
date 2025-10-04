namespace MobCentra.Domain.Entities.Filters
{
    public class ProfileFeatureFilter : SearchParameters<ProfileFeature>
    {
        public Guid? ProfileId { get; set; }
    }
}
