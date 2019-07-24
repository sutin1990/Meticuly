using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_ROUTEINSPECT_SUB
    {
        [Key]
        public string  Code { get; set; }
        public string Name { get; set; }        
        public int ?  DocEntry { get; set; }        
        public string Canceled { get; set; }
        public string Object { get; set; }
        public int ? LogInst { get; set; }
        public int ? UserSign { get; set; }
        public string Transfered { get; set; }
        public DateTime ? CreateDate { get; set; }
        public Int32 ? CreateTime { get; set; }
        public DateTime ? UpdateDate { get; set; }
        public Int32? UpdateTime { get; set; }
        public string DataSource { get; set; }
        public int ? U_DocEntry { get; set; }
        public int ? U_AbsEntry { get; set; }
        public int ? U_Master_DocEntry { get; set; }
        //[Required]
        public int ? U_PassQty { get; set; }
        //[Required]
        public int ? U_RejectQty { get; set; }
        //[Required]
        public string U_RejectReason { get; set; }
        //[Required]
        public string U_Comment { get; set; }
        public string U_Lastupdateby { get; set; }
        public DateTime ? U_Lastupdatedate { get; set; }
        
    }
}
