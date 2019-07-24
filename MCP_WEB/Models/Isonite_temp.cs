using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class Isonite_temp
    {
        [Key]
        [Required]
        public int ID { get; set; }
        public string BarCode { get; set; }
        public int QtyComplete { get; set; }
        public int QtyNG { get; set; }
        public string user_create { get; set; }
        public string Token { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
