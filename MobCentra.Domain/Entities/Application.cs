namespace MobCentra.Domain.Entities
{
    public class Application : BaseEntity<Guid>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public int Active { get; set; }
        public string File { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
