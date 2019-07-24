using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class PartDescTagSelect
    {
        [Required]
        public string Model { set; get; }
        [Key]
        public string PartNo { set; get; }
        public string SerailNoofBarcode { set; get; }
        [Required(ErrorMessage = "Packing Std. ID is required")]
        public string PackingID { set; get; }
        //[Required]
        public string PasrtDesType { set; get; }
        public int? OrderQty { set; get; }
        public int? WIPQTY { set; get; }
        
        [Required]
        [Range(0,int.MaxValue , ErrorMessage = "Packed Qty must be a positive number")]
        public int? QtyPacked { set; get; }
        public int? QTYLeft { set; get; }
    }
}
