using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models {
    public class VW_MFC_T5_AAT {
        // Must Add composite key in DBContext // [Key]
        public string PartNo { get; set; }
        // Must Add composite key in DBContext // [Key]
        public string QR { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Process { get; set; }
        public string Suppiler { get; set; }
        public int? Quantity { get; set; }
        public DateTime? DeliveryDate{ get; set; }
        public string ReceivedLoc { get; set; }
        public string StorageLoc { get; set; }
        public string SupplyLoc { get; set; }
        public string Remark { get; set; }
        public string PartName { get; set; }
        public string PartDesTag { get; set; }
        public string LotNoShow { get; set; }
    }
}
