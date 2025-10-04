using System.ComponentModel.DataAnnotations.Schema;

namespace DcpTracker.Domain.Entities
{
    public class Transaction : BaseEntity<Guid>
    {
        public string Coordinations { get; set; }
        public DateTime TransDateTime { get; set; }
        public Guid? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }
        public Guid? AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public virtual AppUsers AppUser { get; set; }
    }
}
