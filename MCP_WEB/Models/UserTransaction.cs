using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class UserTransaction
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string SessionKey { get; set; }
        public string Token { get; set; }
        public DateTime DateExprie { get; set; }
    }
}
