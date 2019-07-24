using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_ProcessMaster
    {
        [Key]
        public int  IdProcess {get;set;}        
        [Required]
        public string ProcessCode { get; set; }
        [Required]
        public string  ProcessName { get; set; }
        public string  SysemProcessFLag { get; set; }
        public string  AllowPartialFlag { get; set; }
        //[Required]
        public DateTime? TransDate { get; set; }
        //[Required]
        public DateTime? CreateDate { get; set; }
       // [Required]
        public string ModifyBy { get; set; }
        public int ? SeqNo { get; set; }
        public string  ProcessUserDef1 { get; set; }
        public string  ProcessUserDef2 { get; set; }  



    }
}
