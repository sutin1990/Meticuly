using System.Security.Claims;

namespace MCP_WEB.Service
{
    public interface IUserRepository
    {
        bool ValidateLastChanged(ClaimsPrincipal userPrincipal, string lastChanged);
    }
}