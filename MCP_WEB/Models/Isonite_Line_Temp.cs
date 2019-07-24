using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class Isonite_Line_Temp
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
        //public int SentQty { get; set; }
        //public int RecQty { get; set; }
        public string RefIsoniteCode { get; set; }
        public int? RefJigAddress { get; set; }

        public string ReJigNo { get; }

        public string TransType { get; set; }
        public int? Qty { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? TransDate { get; set; }

        public DateTime? Sentdate { get; set; }
        public DateTime? Receivedate { get; set; }

        public string ModifyBy { get; set; }
        public string IsoniteLineUserDef1 { get; set; }
        public string IsoniteLineUserDef2 { get; set; }
        public string IsoniteLineUserDef3 { get; set; }
    }

    public class Isonite_Line_Confirm
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
        //public int SentQty { get; set; }
        //public int RecQty { get; set; }
        public string RefIsoniteCode { get; set; }
        public int? RefJigAddress { get; set; }

        public string ReJigNo { get; set; }

        public string TransType { get; set; }
        public int? Qty { get; set; }

        public int? Confirm_Qty { get; set; }
        public int? NgQty { get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? TransDate { get; set; }

        public DateTime? Sentdate { get; set; }
        public DateTime? Receivedate { get; set; }

        public string ModifyBy { get; set; }
        public string IsoniteLineUserDef1 { get; set; }
        public string IsoniteLineUserDef2 { get; set; }
        public string IsoniteLineUserDef3 { get; set; }
    }
}
