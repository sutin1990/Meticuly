using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_PartDescTagSelect_Calculator
    {
        public string Model { set; get; }
        [Key]
        public string PartNo { set; get; }
        public int SummaryWIPQty { set; get; }
        public int PackQty { set; get; }
        public string PackingId { set; get; }
        public string TagFormat { set; get; }
    }
}
