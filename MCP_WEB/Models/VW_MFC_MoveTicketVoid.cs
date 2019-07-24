using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_MoveTicketVoid
    {
        [Key]
        public string moveTicket { set; get; }
        public string DeliveryNote { set; get; }
        public string BoxType { set; get; }
        public string Model { get; set; }
        [Required]
        public string TagType { set; get; }
        public int QtyMT { set; get; }
        public string StatusMT { set; get; }
        public int NumOfBox { set; get; }
        public int QtyExcess { set; get; }
        public string ItemCode { get; set; }
    }
}
