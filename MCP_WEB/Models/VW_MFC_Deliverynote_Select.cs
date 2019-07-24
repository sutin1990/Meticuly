using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_Deliverynote_Select
    {
        [Key]
        public string DeliveryNote { set; get; }
        public string StatusDelivery { set; get; }
        public string MoveTicket { set; get; }
        public string StatusMT { set; get; }
        public string FCode { set; get; }
        public string Model { set; get; }
        public int MTQty { set; get; }
        public int BoxQty { set; get; }
        public int ExcessQty { set; get; }
        public string Remarks { set; get; }
    }
}
