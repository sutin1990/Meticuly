using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class Isonie_PopupEdit
    {
        [Key]
        public string IsoniteNo { get; set; }
        public string SerialNo { get; set; }
        public string Model { get; set; }
        public int? WIPQty { get; set; }
        public int? ConfirmedQty { get; set; }
        public int? NgQty { get; set; }
    }
}
