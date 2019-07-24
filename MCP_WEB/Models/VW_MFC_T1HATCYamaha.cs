using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models {
    public class VW_MFC_T1HATCYamaha {

        // Must Add composite key in DBContext // [Key]
        public string PartNo { get; set; }
        // Must Add composite key in DBContext // [Key]
        public string QR { get; set; }
        public string PartDesTag { get; set; }
        public string Barcode { get; set; }
        public string PartName { get; set; }
        public DateTime? Date { get; set; }
        public int? Quantity { get; set; }
        public string Weight { get; set; }
        public string OrderNo { get; set; }
        public int? OrderQty { get; set; }
        public string PackType { get; set; }
        public int? TotalPack { get; set; }
        public string ThisPackofTotal { get; set; }
        public string InspectionCode { get; set; }
        public string InspectionBy { get; set; }
        public string CheckBy { get; set; }
        public string LotNo { get; set; }
        public string LotNoShow { get; set; }
        public string Images { get; set; }
        public string Model { get; set; }
        public string Customer { get; set; }
        public string Location { get; set; }
        public string TagColor { get; set; }
        public string BoxColor { get; set; }
    }
}
