using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_Dashboard1
    {
        [Key]
        public string ProcessCode { get; set; }
        public int? All_Work { get; set; }
        public int? Between_Work { get; set; }
        public int? Close_Work { get; set; }
    }
}
