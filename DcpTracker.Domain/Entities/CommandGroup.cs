namespace DcpTracker.Domain.Entities
{
    public class CommandGroup : BaseEntity<Guid>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public int Active { get; set; }
        public virtual ICollection<GoogleCommand> GoogleCommands { get; set; } = [];
    }
}
