using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class IsoniteSum
    {
        [Key]
        [Required]
        public int RECID { get; set; }
        [Required]
        public string IsoniteCode { get; set; }
        [Required]
        public string JigNo { get; set; }
        [Required]
        public int JigAddress { get; set; }
        public string BarCode { get; set; }
        
        public string Model { get; set; }

        public string RefIsoniteCode { get; set; }
        public int? RefJigAddress { get; set; }

        public string TransType { get; set; }
        public int? Qty { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? TransDate { get; set; }
        public string ModifyBy { get; set; }
        public string IsoniteLineUserDef1 { get; set; }
        public string IsoniteLineUserDef2 { get; set; }
        public string IsoniteLineUserDef3 { get; set; }
    }
}
