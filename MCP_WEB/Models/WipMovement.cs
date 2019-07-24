using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class WipMovement
    {
        [Key]
        public int RowNumber { get; set; }
        public string ItemCode { get; set; }
        public string Model { get; set; }
        public string Fcode { get; set; }
        public decimal Dimension1 { get; set; }
        public string Uom { get; set; }
        public string ProcessCode { get; set; }
        public string Reference { get; set; }
        public string Barcode { get; set; }
        public DateTime ? Trdate { get; set; }
        public int BF { get; set; }
        public int TRIN { get; set; }
        public int TROUT { get; set; }
        public int QtyMove { get; set; }
    }
}
