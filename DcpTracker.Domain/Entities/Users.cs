using DcpTracker.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace DcpTracker.Domain.Entities
{
    public class Users : BaseEntity<Guid>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int Active { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<UserCommand> UserCommands { get; set; }
        [NotMapped]
        public string NewPassword { get; set; }
        public Guid? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }
        [NotMapped]
        public OperationType OperationType { get; set; }
        public Guid? CityId { get; set; }

    }
}
