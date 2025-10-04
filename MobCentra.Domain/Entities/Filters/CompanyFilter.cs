namespace MobCentra.Domain.Entities.Filters
{
    public class CompanyFilter : SearchParameters<Company>
    {
        public string Description { get; set; }
        public int? Active { get; set; }

    }
}
