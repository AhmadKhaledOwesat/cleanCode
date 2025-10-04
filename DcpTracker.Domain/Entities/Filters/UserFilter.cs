namespace MobCentra.Domain.Entities.Filters
{
    public class UserFilter : SearchParameters<Users>
    {
        public string UserName { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
