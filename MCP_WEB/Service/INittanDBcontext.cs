using MCP_WEB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MCP_WEB.Service
{
    internal interface INittanDBcontext 
    {
        IEnumerable<UserTransaction> GetUserTransaction();
        IEnumerable<UserTransaction> GetUserTransaction(String UserRole);
    }
}