using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Service
{
    public class InterFaceDBcontext : INittanDBcontext
    {

        private readonly NittanDBcontext _context;

        public InterFaceDBcontext(NittanDBcontext context)
        {
            this._context = context;
        }

        public IEnumerable<UserTransaction> GetUserTransaction()
        {
            return _context.UserTransaction.AsEnumerable();
        }

        public IEnumerable<UserTransaction> GetUserTransaction(string UserRole)
        {
            var result = (from a in _context.UserTransaction
                          where a.UserName == UserRole
                          select a);

            return result;
        }
    }
}
