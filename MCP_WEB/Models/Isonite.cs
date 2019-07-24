using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class Isonite
    {
        [Key]
        [Required]
        public string IsoniteCode { get; set; }
        [Required]
        public string BPCodeFrom { get; set; }
        [Required]
        
        public string BPCodeTO { get; set; }
        public string JigNo1 { get; set; }
        public string JigNo2 { get; set; }
        public string JigNo3 { get; set; }

        public DateTime? DocDate { get; set; }
        public string DocStatus { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? TransDate { get; set; }
        
        public string ModifyBy { get; set; }
        public string IsoniteUserDef1 { get; set; }
        public string IsoniteUserDef2 { get; set; }
        public string IsoniteUserDef3 { get; set; }
    }
}
