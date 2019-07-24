using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models {
    public class m_Jig {

        [Key]
        public string JigNo { get; set; }

        public int JigID { get; set; }
       
        [Range(1, 4, ErrorMessage = "Can't Insert Column more than 4")]
        public int Column { get; set; }
        [Range(1, 14, ErrorMessage = "Can't Insert Row more than 14")]
        public int Row { get; set; }
        public string JigUserDef1 { get; set; }
        public string JigUserDef2 { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }

        public string jig_isonite_status { get; set; }

    }
}
