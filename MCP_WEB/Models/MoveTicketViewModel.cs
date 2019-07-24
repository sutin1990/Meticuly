using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class MoveTicketViewModel
    {
        [Key]
        public string moveTicket { set; get; }
        public string DeliveryNote { set; get; }
        public string BoxType { set; get; }
        [Required]
        public string TagType { set; get; }
        public int QtyMT { set; get; }
        public string StatusMT { set; get; }
        public int NumOfBox { set; get; }
        public int QtyExcess { set; get; }
        public string Remarks { set; get; }
        public DateTime CreateDate { set; get; }
        public DateTime TransDate { set; get; }
        public string ModifyBy { set; get; }
        public string MoveTicketUserDef1 { set; get; }
        public string MoveTicketUserDef2 { set; get; }
        public string MoveTicketUserDef3 { set; get; }

        public string SqlStatus { get; set; }
        public string SqlErrtext { get; set; }

        public string Model { get; set; }
        public int QtyOrder { get; set; }
        public int WIPQTY { get; set; }
        public string ItemCode { get; set; }


        public string BoxNo { set; get; }
        public string Barcode { set; get; }
        public int QtyLot { set; get; }
        public string StatusLot { set; get; }
    }
}
