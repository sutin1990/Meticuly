using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class WIPStatus
    {
        [Key]
        public int RowNumber { get; set; }        
        public DateTime ? Asof { get; set; }        
        public string MachineCode { get; set; }
        public string Fcode { get; set; }
        public string Model { get; set; }                
        public int CUTOFFBAR { get; set; }
        public int FRICTION { get; set; }
        public int FORGING { get; set; }
        public int STELLITE_TIP { get; set; }
        public int STELLITE_SEAT { get; set; }
        public int STRAIGHTENING { get; set; }
        public int STEM_ROUGH { get; set; }
        public int STEM_FINISH { get; set; }
        public int ISONITE { get; set; }
        public int SEAT_FINISH { get; set; }
        public int QC { get; set; }       
        public string QtyMove { get; set; }

    }
}
