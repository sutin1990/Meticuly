using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Profile
{
    public class ProfileManager : Controller
    {
        public string CurrentUser()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var c = claims.FirstOrDefault();

            return c.Value;
        }
    }
}
