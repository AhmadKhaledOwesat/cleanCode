namespace DcpTracker.Domain.Entities
{
    public class TaskStatus : BaseEntity<Guid>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public int Active { get; set; }
        public int MobileAppDisplay { get; set; }
        public int BackOfficeDisplay { get; set; }
    }
}
