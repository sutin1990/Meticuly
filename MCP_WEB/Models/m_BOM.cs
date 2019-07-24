using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_BOM
    {
        [Key]
        [Required]
        public string ItemCode { get; set; }
        public string Fcode { get; }
        public string ItemName { get; }
        public string Model { get; }
        public string Material1 { get; set; }
        public string Material2 { get; set; }
        public string BOMUserDef1 { get; set; }
        public string BOMUserDef2 { get; set; }
        public string BOMUserDef3 { get; set; }
        public string BOMUserDef4 { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
