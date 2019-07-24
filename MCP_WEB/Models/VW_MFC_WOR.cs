using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_WOR
    {
        [Key]
        public int DocEntry { get; set; }
        public int StageId { get; set; }
        public int SeqNum { get; set; }
        public int StgEntry { get; set; }
        public string Name { get; set; }
        public string LogInstanc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int RtCalcProp { get; set; }
        public int ReqDays { get; set; }
        public int WaitDays { get; set; }

    }
}
