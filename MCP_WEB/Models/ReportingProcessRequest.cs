using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class ReportingProcessRequest
    {
        public string ProcessText { get; set; }
        public string Barcode { get; set; }
        public Int32? OperationNo { get; set; }
        public Int32? ShiftID { get; set; }
        public string PStatus { get; set; }
        public Int32? QtyComplete { get; set; }
        public Int32? QtyNG { get; set; }
        public string MachineCode { get; set; }
    }
}
