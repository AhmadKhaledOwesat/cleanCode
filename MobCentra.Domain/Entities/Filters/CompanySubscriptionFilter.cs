namespace MobCentra.Domain.Entities.Filters
{
    public class CompanySubscriptionFilter : SearchParameters<CompanySubscription>
    {
        public string Description { get; set; }
    }
}
