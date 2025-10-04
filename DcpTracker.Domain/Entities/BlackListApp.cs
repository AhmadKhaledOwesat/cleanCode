namespace DcpTracker.Domain.Entities
{
    public class BlackListApp : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int IsBlackList { get; set; }
        public int IsActive { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
