using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class s_ProcessLog
    {
        [Key]
        [Required(ErrorMessage = "ID is Required.")]
        public int? id { get; set; }
        [Required(ErrorMessage = "Process ID is Required.")]
        public string ProcessID { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int? ErrorKey { get; set; }
        public string RecordKey1 { get; set; }
        public string RecordKey2 { get; set; }
        public string Msg { get; set; }
    }
}
