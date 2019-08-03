using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_OWOR
    {
        [Key]
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string ItemCode { get; set; }       
        public string Status { get; set; }       

    }
}
