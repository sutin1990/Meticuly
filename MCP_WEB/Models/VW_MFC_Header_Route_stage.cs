using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_Header_Route_stage
    {
        [Key]
        public int DocEntry { get; set; }
        [Required]
        public int DocNum { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public int ? OriginNum { get; set; }
        [Required]
        public string  ItemCode { get; set; }
        [Required]
        public string ProdName { get; set; }
        [Required]
        public decimal ? PlannedQty { get; set; }
        [Required]
        public string  Uom { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        
    }
}
