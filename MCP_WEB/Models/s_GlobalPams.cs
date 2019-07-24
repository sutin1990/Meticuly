using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class s_GlobalPams
    {
        [Key]
        public string parm_key { get; set; }
        public string parm_desc { get; set; }
        public string opt1 { get; set; }
        public string opt2 { get; set; }
        public string param_value { get; set; }
    }
}
