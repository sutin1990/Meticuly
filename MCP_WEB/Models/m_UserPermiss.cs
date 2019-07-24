using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_UserPermiss
    {
        [Key]
        public int PerID { get; set; }
        [Required]
        public string UserId { get; set; }        
        public int MenuIdentity { get; set; } 
        [DataType(DataType.Date)]
        public DateTime? TransDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreateDate { get; set; }
        [Required]
        public string ModifyBy { get; set; }
    }
}
