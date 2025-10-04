namespace MobCentra.Domain.Entities.Filters
{
    public class UserCommandFilter : SearchParameters<UserCommand>
    {
        public Guid? UserId { get; set; }   
    }
}
