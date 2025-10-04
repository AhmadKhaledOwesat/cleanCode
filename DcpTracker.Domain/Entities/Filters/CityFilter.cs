namespace DcpTracker.Domain.Entities.Filters
{
    public class CityFilter : SearchParameters<City>
    {
        public string Description { get; set; }
        public int? Active { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
