using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_Package
    {
        [Key]
        [Required(ErrorMessage = "Pack ID is Required.")]
        public string PackID { get; set; }
        [Required(ErrorMessage = "Pack Description is Required.")]
        public string PackDesc { get; set; }
        [Required(ErrorMessage = "Pack Type is Required.")]
        public string PackType { get; set; }
        [Required(ErrorMessage = "Pack Quantity is Required.")]
        public Int32 PackQty { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
    }

    public class m_packageType
    {
        [Key]
        public string PackageTypeTd { get; set; }
        public string PackageTypeName { get; set; }
    }
        
}
