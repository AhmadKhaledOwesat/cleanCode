namespace MobCentra.Domain.Entities.Filters
{
    public class FeatureFilter : SearchParameters<Feature>
    {
        public string Description { get; set; }
    }
}
