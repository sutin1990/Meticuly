using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class WIPProcessBalance
    {
        [Key]
        public int RowNumber { get; set; }
        public DateTime Asof { get; set; }        
        public string Fcode { get; set; }
        public string Model { get; set; }                
        public string Material1 { get; set; }
        public string ItemName { get; set; }
        public string BarCode { get; set; }
        public string MachineCode { get; set; }        
        public int QtyMove { get; set; }
        public string ProcessCode { get; set; }

    }
}
