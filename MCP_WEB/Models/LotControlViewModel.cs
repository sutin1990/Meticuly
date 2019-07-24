using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models {
    public class LotControlViewModel {
        public WeeklyPlan WeeklyPlanVM { get; set; }
        public WoBOM wobomVM { get; set; }
        public m_Resource m_resourceVM { get; set; }
        public m_BOM m_BomVM { get; set; }
        public m_BPMaster m_BPMasterVM { get; set; }

        //public WoRouting WoRoutingVM { get; set; }
        public IEnumerable<WoRouting> WoRoutingVM { get; set; }
        //public List<WoRouting> WoRoutingVM { get; set; }
        public string BPAddress6 { get; set; }
        public virtual int RowNum { get; set; }
        public virtual int PageNum { get; set; }
        public virtual string Image1 { get; set; }
}
}
