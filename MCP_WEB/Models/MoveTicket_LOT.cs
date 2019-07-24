using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class MoveTicket_LOT
    {
        [Key]
       public int RECID { set; get; }
       public string MoveTicket { set; get; }
       public string BoxNo { set; get; }
       public string Barcode { set; get; }
       public int QtyLot { set; get; }
       public string StatusLot { set; get; }
       public string Remarks { set; get; }
       public DateTime CreateDate { set; get; }
       public DateTime TransDate { set; get; }
       public string ModifyBy { set; get; }
       public string MoveTicketLOTUserDef1 { set; get; }
       public string MoveTicketLOTUserDef2 { set; get; }
       public string MoveTicketLOTUserDef3 { set; get; }
    }
}
