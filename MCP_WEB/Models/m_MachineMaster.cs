using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_MachineMaster
    {
        [Key]
        [Required(ErrorMessage = "Machine code is Required.")]
        public string MachineCode { get; set; }
        [Required(ErrorMessage = "Machine name is Required.")]
        public string MachineName { get; set; }
        public string MachineUserDef1 { get; set; }
        public string MachineUserDef2 { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
