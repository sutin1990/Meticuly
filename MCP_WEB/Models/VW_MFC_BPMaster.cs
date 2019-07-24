using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_BPMaster
    {
        [Key]        
        public string BPCode { get; set; }        
        public string BPName { get; set; }        
        public string BPType { get; set; }
        public string BPAddress1 { get; set; }
        public string BPAddress2 { get; set; }
        public string BPAddress3 { get; set; }
        public string BPAddress4 { get; set; }
        public string BPAddress5 { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string BPAddress6 { get; set; }
        public string TagFormat { get; set; }
        public string PackingID { get; set; }
    }
}
