using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class IsoniteViewModel
    {
        public Isonite IsoniteVM { get; set; }
        public VW_MFC_Isonite_Line Isonite_LineVM { get; set; }
        public VW_MFC_JigonIsonite VW_MFC_JigonIsonite { get; set; }
        public WeeklyPlan WeeklyPlanVM { get; set; }
        public WoRouting WoRoutingVM { get; set; }
        public m_BPMaster m_BPMaster1VM { get; set; }
        public m_BPMaster m_BPMaster2VM { get; set; }
        public m_Jig m_JigVM { get; set; }

        public string IsoniteCode { get; set; }
        public string addr6 { get; set; }
        public string namefrom { get; set; }
        public string addrfrom { get; set; }
        public string nameto { get; set; }
        public string addrto { get; set; }

        public string jigno { get; set; }
        public string jigIsoniteCode { get; set; }

    }
}
