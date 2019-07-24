using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_DepMenu
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "DepID is Required.")]
        public string DepID { get; set; }   
        [Required]
        public int MenuIdentity { get; set; }
        [Required]
        public DateTime? TransDate { get; set; }
        [Required]
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }

        //public string MenuName { get; set; }
        public virtual MenuMaster MenuMaster { get; set; }
    }
}
