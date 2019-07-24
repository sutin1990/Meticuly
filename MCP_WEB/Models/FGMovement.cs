using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class FGMovement
    {
        [Key]        
        public int RowNumber { get; set; }
        public string FCode { get; set; }
        public string Model { get; set; } 
        public string Uom { get; set; }       
        public string BarCode { get; set; }
        public DateTime TransDate { get; set; }
        public int qty { get; set; }
        
    }
}
