namespace DcpTracker.Domain.Entities.Filters
{
    public class CommandGroupFilter : SearchParameters<CommandGroup>
    {
        public string Description { get; set; }
        public int? Active { get; set; }
    }
}
