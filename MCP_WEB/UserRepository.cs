using System.Security.Claims;
using MCP_WEB.Service;

namespace MCP_WEB
{
    internal class UserRepository : IUserRepository
    {
        public bool ValidateLastChanged(ClaimsPrincipal userPrincipal, string lastChanged)
        {
            throw new System.NotImplementedException();
        }
    }
}