using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class DCWIPbyShiftH
    {
        [Key]       
        public int RowNumber { get; set; }
        public string ProcessCode { get; set; }
        public string Barcode { get; set; }
        public int QtyOrder { get; set; }
        public decimal WIP { get; set; }
        public decimal NG { get; set; }
        public decimal NC { get; set; }
        public decimal WIPRate { get; set; }
        public decimal NGRate { get; set; }
        public decimal NCRate { get; set; }

    }
}
