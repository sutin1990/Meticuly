using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models {
    public class VW_MFC_T4_THM_GD_TSM {

        // Must Add composite key in DBContext // [Key]
        public string  PartNo { get; set; }
        // Must Add composite key in DBContext // [Key]
        public string QR { get; set; }
        public string  PartName { get; set; }
        public string  Model { get; set; }
        public int?  OrderQty { get; set; }
        public int QuantityLot { get; set; }
        public string  Color { get; set; }
        public string  LotNo { get; set; }
        public string  Location { get; set; }
        public string  Customer { get; set; }
        public string  PlantCode { get; set; }
        public string  Timeofdelivery { get; set; }
        public string  _By { get; set; }
        public DateTime? Date0 { get; set; }
        public string  PackBy { get; set; }
        public string  CheckBy { get; set; }
        public string  StoreBy { get; set; }
        public string  ReceiveNo { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }
        public DateTime?  Date4 { get; set; }
        public string  PartDesTag { get; set; }
        public string LotNoShow { get; set; }
        

    }
}
