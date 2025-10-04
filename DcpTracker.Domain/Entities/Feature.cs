namespace MobCentra.Domain.Entities
{
    public class Feature : BaseEntity<int>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public int Active { get; set; }
        public string Icon { get; set; }    
        public string Action { get; set; }
    }
}
