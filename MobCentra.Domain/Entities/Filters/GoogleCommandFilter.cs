namespace MobCentra.Domain.Entities.Filters
{
    public class GoogleCommandFilter : SearchParameters<GoogleCommand>
    {
        public string Description { get; set; }
        public int? ShowInSetupForm { get; set; }

    }
}
