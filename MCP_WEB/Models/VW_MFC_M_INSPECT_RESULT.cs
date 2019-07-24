using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_M_INSPECT_RESULT
    {
        [Key]
        public int ? Code { get; set; }
        public string Name { get; set; }
        public int ? U_DocEntry { get; set; }
        public int ? U_DocNum { get; set; }
        public int ? U_StageId { get; set; }
        public int ? U_SeqNum { get; set; }
        public int ? U_StgEntry { get; set; }
        public string U_Name { get; set; }
        public int ? U_InspectEntry { get; set; }
        public string U_InspectMethod { get; set; }
        public int ? U_RptOrdering { get; set; }
        public int ? U_PassQty { get; set; }
        public int ? U_RejQty { get; set; }
        public string U_Reason { get; set; }
        public string U_Comment { get; set; }        
        public DateTime ? U_CreateDate { get; set; }
        public string U_CreateBy { get; set; }
        public DateTime ? U_UpdateDate { get; set; }
        public string U_UpdateBy { get; set; }      
        
    }
}
