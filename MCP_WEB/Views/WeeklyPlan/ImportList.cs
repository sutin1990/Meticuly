using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Views.WeeklyPlan
{
    public class ImportList
    {
        public int id { get; set; } //external from proc
        public string ProcessID { get; set; }
        public DateTime ProcessDate { get; set; } //external from proc
        public string Msg { get; set; } //external from proc

        public int? ErrorKey { get; set; }
        public string RecordKey1 { get; set; }
        public string RecordKey2 { get; set; }
        
        public string Confirm { get; set; }

        public string FCode { get; set; }
        public string ItemCode { get; set; }
        public Int32 QtyOrderAll { get; set; }
        public string SeriesLot { get; set; }
    }
}
