namespace MobCentra.Domain.Entities.Filters
{
    public class AppUserFilter : SearchParameters<AppUsers>
    {
        public string FullName { get; set; }
    }
}
