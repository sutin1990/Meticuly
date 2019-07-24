using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models {
    public class WoBOM {
        [Key]
        public string BarCode { get; set; }
        public string Material1 { get; set; }
        public string Material2 { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
