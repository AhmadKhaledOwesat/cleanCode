namespace MobCentra.Domain.Entities.Filters
{
    public class ProfileFeatureFilter : SearchParameters<ProfileFeature>
    {
        public Guid? ProfileId { get; set; }
    }

    public class ProfileApplicationFilter : SearchParameters<ProfileApplication>
    {
        public Guid? ProfileId { get; set; }
    }
}
