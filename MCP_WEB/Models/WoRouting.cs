using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class WoRouting
    {
        [Key]
        [Column(Order = 0)]
        public string BarCode { get; set; }
        [Key]
        [Column(Order = 1)]
        public int OperationNo { get; set; }

        public string ProcessCode { get; set; }
        public string MachineCode { get; set; }
        public int? QtyOrder { get; set; }
        public int? QtyComplete { get; set; }
        public int? QtyNG { get; set; }
        public string AllowPartialFlag { get; set; }
        public string MainProcessFlag { get; set; }
        public string PStatus { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }

        //edit by Tar
        public int? QTYinProcess { get; set; }
    }
}
