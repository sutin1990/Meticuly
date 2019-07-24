using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models {
    public class VW_MFC_T3_ZR {

        // Must Add composite key in DBContext // [Key]
        public string PartNo { get; set; }
        // Must Add composite key in DBContext // [Key]
        public string QR { get; set; }
        public string PartName { get; set; }
        public string SuppilerName { get; set; }
        public string Model { get; set; }
        public string LotControl { get; set; }
        public string CheckBy { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string SupplierApproved { get; set; }
        public int? Quantity { get; set; }
        public string StoredBy { get; set; }
        public string ReceivedNo { get; set; }
        public string PartDesTag { get; set; }
        public string LotNoShow { get; set; }
    }
}
