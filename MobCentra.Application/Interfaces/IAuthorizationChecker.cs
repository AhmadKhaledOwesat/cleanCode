namespace MobCentra.Application.Interfaces
{
    public interface IAuthorizationChecker
    {
        Task<bool> HasPermissionAsync(Guid userId, Guid privilegeId);
    }
}
