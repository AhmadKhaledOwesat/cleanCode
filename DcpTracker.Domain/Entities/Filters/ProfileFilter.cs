namespace MobCentra.Domain.Entities.Filters
{
    public class ProfileFilter : SearchParameters<Profile>
    {
        public string Description { get; set; }
        public int? Active { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
