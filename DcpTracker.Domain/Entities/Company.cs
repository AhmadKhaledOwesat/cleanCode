namespace MobCentra.Domain.Entities
{
    public class Company : BaseEntity<Guid>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public string ContactPerson { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyCoordination { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public int Active { get; set; }
        public int? NoOfDevices { get; set; }
        public int? NoOfUsers { get; set; }
        public int? NoOfRoles { get; set; }
        public int? NoOfGroups { get; set; }
        public int? NoOfArea { get; set; }
        public int? NoOfProfile { get; set; }
        public int? NoOfNotifications { get; set; }
        public int? NoOfApplications { get; set; }
        public int? NoOfTrackedDevices { get; set; }
        public virtual ICollection<AppUsers> AppUsers { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
