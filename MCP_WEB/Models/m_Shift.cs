using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_Shift
    {
        [Key]
        [Required(ErrorMessage = "Shift ID is Required.")]
        public int ShiftID { get; set; }
        [Required(ErrorMessage = "Shift Type is Required.")]
        public string ShiftType { get; set; }
        public string ShiftDesc { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
