using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_Resource
    {
        [Key]
        [Required(ErrorMessage = "ItemCode is Required.")]
        public string ItemCode { get; set; }
        [Required(ErrorMessage = "ItemName is Required.")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "Model is Required.")]
        public string Model { get; set; }
        [Required(ErrorMessage = "F-code is Required.")]
        public string Fcode { get; set; }
        [Required(ErrorMessage = "BP Name is Required.")]
        public string BPCode { get; set; }
        public string ItemType { get; set; }
        public int? StdLotSize { get; set; }/*Paisarn add ?*/
        public int? Tolerance { get; set; }/*Paisarn add ?*/
        public string Status { get; set; }
      
        public decimal? Dimension1 { get; set; }
        
        public decimal? Dimension2 { get; set; }
        public string Color { get; set; }
        public int? Length { get; set; } /*Paisarn add ?*/
        public string Batch1 { get; set; }
        public string Batch2 { get; set; }
        public string Uom { get; set; }
        public string HeatNo { get; set; }
        public string LengthUom { get; set; }
        public string ItemUserDef1 { get; set; }
        public string ItemUserDef2 { get; set; }
        public string ItemUserDef3 { get; set; }
        public string ItemUserDef4 { get; set; }
        public string ItemUserDef5 { get; set; }
        public string ItemImage { get; set; }
        public DateTime? TransDate { get; set; } /*Paisarn add ?*/
        public DateTime? CreateDate { get; set; } /*Paisarn add ?*/
        public string ModifyBy { get; set; }
       
    }
}
