namespace DcpTracker.Domain.Entities.Filters
{
    public class UserGroupFilter : SearchParameters<UserGroup>
    {
        public Guid? UserId { get; set; }   
    }
}
