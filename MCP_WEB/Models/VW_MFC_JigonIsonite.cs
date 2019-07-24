using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_JigonIsonite
    {
        [Key]
        public string JigNo { set; get; }
        public string IsoniteCode { set; get; }
    }
}
