using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_OSRN
    {
        [Key]
        public int ? AbsEntry { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public Int16 UserSign { get; set; }
        public Int16 UserSign2 { get; set; }        
        public DateTime CreateDate { get; set; }
        public Int16 CreateTime { get; set; }
        public DateTime UpdateDate { get; set; }
        public int LogInstanc { get; set; }

    }
}
