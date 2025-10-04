using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
{
    public class UserCommand : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Users User { get; set; }

        public Guid GoogleCommandId { get; set; }
        [ForeignKey(nameof(GoogleCommandId))]
        public virtual GoogleCommand GoogleCommand { get; set; }  
    }
}
