using DcpTracker.Application.Dto;

namespace DcpTracker.Application.Interfaces
{
   public interface IAuthenticationManager
    {
        Tokens GenerateToken(string userName, object id);
    }
}
