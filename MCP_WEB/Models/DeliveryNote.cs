using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class DeliveryNote
    {
        [Key]
        public string deliverynote { set; get; }
        public string StatusDelivery { set; get; }
        public string Remarks { set; get; }
        public DateTime CreateDate { set; get; }
        public DateTime TransDate { set; get; }
        public string ModifyBy { set; get; }
        public string DeliveryUserDef1 { set; get; }
        public string DeliveryUserDef2 { set; get; }
        public string DeliveryUserDef3 { set; get; }

    }
}
