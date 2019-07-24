using MCP_WEB.Data;
using MCP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MCP_WEB.Services
{
    public class MenuMasterService:IMenuMasterService
    {
		private readonly NittanDBcontext _dbContext;

		public MenuMasterService(NittanDBcontext dbContext)
		{
			_dbContext = dbContext;
		}

		public IEnumerable<MenuMaster> GetMenuMaster()
		{ 
			return _dbContext.MenuMaster.AsEnumerable();

		}

        public IEnumerable<MenuMaster> GetMenuMasterParent(string UserRole)
        {
            var result = _dbContext.MenuMaster.Where(m => m.User_Roll == UserRole).ToList();
            return result;
        }
        public IEnumerable<MenuMaster> GetMenuMaster(string UserRole)
		{
            //var result = _dbContext.MenuMaster.Where(m => m.User_Roll == UserRole).ToList();
            //return result;
            var result = (from a in _dbContext.m_UserMaster
                          join b in _dbContext.m_UserPermiss on a.UserId equals Int32.Parse(b.UserId)
                          join c in _dbContext.MenuMaster on b.MenuIdentity equals c.MenuIdentity
                          where a.UserName == UserRole
                          select c
                    );

            return result;
        }
	}
}
