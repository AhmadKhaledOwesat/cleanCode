namespace DcpTracker.Domain.Entities.Filters
{
    public class GroupFilter : SearchParameters<Group>
    {
        public string Description { get; set; }
        public int? Active { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
