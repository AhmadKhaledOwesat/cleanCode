using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
{
    public class Privilege : BaseEntity<Guid>
    {
        public string PrivilegeName { get; set; }
        public string PrivilegeNameEn { get; set; }
        public Guid? ParentId { get; set; }
        public string Icon { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Privilege Parent { get; set; }  
        public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }
    }
}
