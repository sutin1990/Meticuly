using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class jig_status
    {
        [Key]
        [Required]
        public int ID { get; set; }
        public string jig_id { get; set; }
        public string status { get; set; }
    }
}
