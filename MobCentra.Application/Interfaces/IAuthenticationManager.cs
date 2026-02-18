using MobCentra.Application.Dto;

namespace MobCentra.Application.Interfaces
{
    public interface IAuthenticationManager
    {
        Tokens GenerateToken(string userName, object id);
        /// <summary>Validates the current bearer token and returns a new token with extended expiry.</summary>
        Tokens RefreshToken(string bearerToken);
    }
}
