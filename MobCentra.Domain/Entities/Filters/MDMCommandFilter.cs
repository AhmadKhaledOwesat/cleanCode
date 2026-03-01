namespace MobCentra.Domain.Entities.Filters
{
    public class MDMCommandFilter : SearchParameters<MDMCommand>
    {
        public string Description { get; set; }
        public int? ShowInSetupForm { get; set; }

    }
}
