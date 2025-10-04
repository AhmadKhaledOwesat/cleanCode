namespace DcpTracker.Domain.Entities
{
    public class Version : BaseEntity<Guid>
    {
        public string VersionName { get; set; }
        public string VersionNumber { get; set; }
        public string VersionLink { get; set; }
    }
}
