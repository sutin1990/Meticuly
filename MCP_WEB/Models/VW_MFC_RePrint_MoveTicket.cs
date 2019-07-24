using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_RePrint_MoveTicket
    {
        [Key]
        public string MoveTicket { get; set; }
        public string StatusMT { get; set; }
        public string Model { get; set; }
        public string PartNo { get; set; }
        public string FCode { get; set; }
        public string PackingID { get; set; }
        public string PartDesType { get; set; }

    }
}
