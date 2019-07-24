using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_ProductionDailyReport1
    {
        [Key]
        public int RowNumber { set; get; }
        public string FCode { set; get; }
        public string Model { set; get; }
        public int CUTOFFBAR { set; get; }
        public int FRICTION { set; get; }
        public int FORGING { set; get; }
        public int STELLITETIP { set; get; }
        public int STELLITESEAT { set; get; }
        public int STRAIGHTENING { set; get; }
        public int STEMROUGH { set; get; }
        public int STEMFINISH { set; get; }
        public int ISONITE { set; get; }
        public int SEATFINISH { set; get; }
        public int QCVISUAL { set; get; }
        public int Total { set; get; }
    }
}
