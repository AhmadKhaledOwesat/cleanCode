namespace MobCentra.Domain.Entities.Filters
{
    public class GoogleCommandFilter : SearchParameters<GoogleCommand>
    {
        public string Description { get; set; }
    }
}
