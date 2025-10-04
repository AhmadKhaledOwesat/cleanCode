using MobCentra.Application.Dto;

namespace MobCentra.Application.Interfaces
{
   public interface IAuthenticationManager
    {
        Tokens GenerateToken(string userName, object id);
    }
}
