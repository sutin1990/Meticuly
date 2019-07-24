using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class m_Reason
    {
        [Key]
        public int ID { set; get; }
        public string ReasonID { set; get; }
        public string DepDesc { set; get; }
        public DateTime ? TransDate { set; get; }
        public DateTime ? CreateDate { set; get; }
        public string  ModifyBy { set; get; }
        public string ReasonUserDef1 { set; get; }
        public string ReasonUserDef2 { set; get; }
        public string ReasonUserDef3 { set; get; }

    }
}
