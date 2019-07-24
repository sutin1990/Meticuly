using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_Routing
    {
        [Key]
        [Column(Order = 0)]
        [Required(ErrorMessage = "ItemCode is Required.")]
        public string ItemCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [Required(ErrorMessage = "Operation No is Required.")]
        [RegularExpression(@"([0-9]+)", ErrorMessage = "Must be a Number.")]
        public int OperationNo { get; set; }
        [Required(ErrorMessage = "Process is Required.")]
        public string ProcessCode { get; set; }
        public string MachineCode { get; set; }
        public decimal? PiecePerMin { get; set; }
        //Additional
        public string PartName { get; }
        public string Model { get; }

        public string Menage { get; }

        public string Fcode { get;}
        public DateTime? TransDate { get; set; } /*Paisarn add ?*/
        public DateTime? CreateDate { get; set; } /*Paisarn add ?*/
        public string ModifyBy { get; set; }

    }
}
