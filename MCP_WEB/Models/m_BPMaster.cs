using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_BPMaster
    {
        [Key]
        [Required(ErrorMessage = "BP Code is Required.")]
        public string BPCode { get; set; }
        [Required(ErrorMessage = "BP Name is Required.")]
        public string BPName { get; set; }
        [Required(ErrorMessage = "BP Type is Required.")]
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

    public class m_BPType
    {
        public string TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
