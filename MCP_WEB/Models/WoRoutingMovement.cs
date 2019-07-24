using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class WoRoutingMovement
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string BarCode { get; set; }
        public int? OperationNo { get; set; }
        public int? ShiftID { get; set; }
        public int? QtyComplete { get; set; }
        public int? QtyNG { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
