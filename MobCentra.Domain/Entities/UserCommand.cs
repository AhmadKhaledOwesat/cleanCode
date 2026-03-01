using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
{
    public class UserCommand : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Users User { get; set; }

        public Guid MDMCommandId { get; set; }
        [ForeignKey(nameof(MDMCommandId))]
        public virtual MDMCommand GoogleCommand { get; set; }  
    }
}
