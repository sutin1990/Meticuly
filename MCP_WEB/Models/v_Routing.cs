using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class v_Routing
    {
        [Key]
        [Required(ErrorMessage = "FCode is Required.")]
        public string FCode { get; set; }
        [Required(ErrorMessage = "Operation No is Required.")]
        public int? OperationNo { get; set; }
        [Required(ErrorMessage = "Process is Required.")]
        public string ProcessCode { get; set; }
        public string MachineCode { get; set; }
        public decimal? PiecePerMinStd { get; set; }
        //Additional
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string Model { get; set; }

        public DateTime? TransDate { get; set; } /*Paisarn add ?*/
        public DateTime? CreateDate { get; set; } /*Paisarn add ?*/
        public string ModifyBy { get; set; }

    }
}
