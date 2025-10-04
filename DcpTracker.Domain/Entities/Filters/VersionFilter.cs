namespace DcpTracker.Domain.Entities.Filters
{
    public class VersionFilter : SearchParameters<Version>
    {
        public string Name { get; set; }
    }
}
