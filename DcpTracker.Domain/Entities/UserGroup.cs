using System.ComponentModel.DataAnnotations.Schema;

namespace DcpTracker.Domain.Entities
{
    public class UserGroup : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Users User { get; set; }
        public Guid GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }  
    }
}
