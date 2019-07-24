using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_WIPAdjust
    {
        [Key]
        [Required]
        public string BarCode { get; set; }
        [Required]
        public int Operation { get; set; }
        [Required]
        public string ProcessCode { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int WIPQty { get; set; }


        [Required]
       virtual public int QtyDiff { get; set; }
        [Required]
        virtual public int QtyAudit { get; set; }
    }
}
