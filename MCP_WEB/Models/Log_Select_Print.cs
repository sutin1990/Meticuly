using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class Log_Select_Print
    {
        [Key]
        public int id { set; get; }
        public string opt { set; get; }
        public string name { set; get; }
        public string ipaddress { set; get; }
        //public DateTime ? fromdate { set; get; }
        //public DateTime ? todate { set; get; }
    }
}
