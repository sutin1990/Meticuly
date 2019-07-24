using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_Dep
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string DepID { get; set; }
        [Required]
        public string DepDesc { get; set; }      
        [Required]
        public DateTime? TransDate { get; set; }
        [Required]
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
                
    }
}
