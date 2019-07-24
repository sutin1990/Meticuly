using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class ReportingProcess
    {
        public String mProcessText { get; set; }
        [Key]
        public string Barcode { get; set; }
        public Int32? OperationNo { get; set; }
        public string ProcessCode { get; set; }
        public string Model { get; set; }
        public string Material1 { get; set; }
        public string Material2 { get; set; }
        public string MachineCode { get; set; }
        public string AllowPartialFlag { get; set; }
        public string PStatus { get; set; }
        public Int32? ShiftID { get; set; }
        public Int32? QtyOrder { get; set; }
        public Int32? QtyComplete { get; set; }
        public Int32? QtyNG { get; set; }
        public Int32? QtyNC { get; set; }
        public Int32? TOQtyComplete { get; set; }
        public Int32? TOQtyNG { get; set; }
        public Int32? PPQtyComplete { get; set; }
        public Int32? PPQtyNG { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }

        public string SqlStatus { get; set; }
        public string SqlErrtext { get; set; }

        public string PreProcess { get; set; }
    }
}
