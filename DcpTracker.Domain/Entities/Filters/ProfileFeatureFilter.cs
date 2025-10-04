namespace DcpTracker.Domain.Entities.Filters
{
    public class ProfileFeatureFilter : SearchParameters<ProfileFeature>
    {
        public Guid? ProfileId { get; set; }
    }
}
