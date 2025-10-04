using DcpTracker.Domain.Enum;

namespace DcpTracker.Application.Dto
{
    public class UsersDto : BaseDto<Guid>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int Active { get; set; }
        public string Token { get; set; }
        public virtual ICollection<UserRoleDto> UserRoles { get; set; }
        public virtual ICollection<UserCommandDto> UserCommands { get; set; }
        public virtual ICollection<UserGroupDto> UserGroups { get; set; }
        public virtual Guid[] Permssions { get; set; }
        public string NewPassword { get; set; }
        public Guid? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameOt { get; set; }
        public string CompanyCoordination { get; set; }
        public string EndDate { get; set; }
        public OperationType OperationType { get; set; }
        public Guid? CityId { get; set; }   
    }
}
