using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_NextNumber
    {
        [Key]
        [Column(Order = 0)]
        public string FieldName { get; set; }
        [Key]
        [Column(Order = 1)]
        public string opt1 { get; set; }
        [Key]
        [Column(Order = 2)]
        public string opt2 { get; set; }

        public int NextNumber { get; set; }
    }
}
