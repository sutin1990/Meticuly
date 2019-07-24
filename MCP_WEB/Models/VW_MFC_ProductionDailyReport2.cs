using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_ProductionDailyReport2
    {
        [Key]
        public string FCode { get; set; }
        public string Model { get; set; }
        public string ProcessCode { get; set; }
        //public int QtyComplete { get; set; }
        //public string MachineName { get; set; }
        public int ? MC1 { get; set; }
        public int  MC2 { get; set; }
        public int  MC3 { get; set; }
        public int  MC4 { get; set; }
        public int  MC5 { get; set; }
        public int  MC6 { get; set; }
        public int  MC7 { get; set; }
        public int  MC8 { get; set; }
        public int  MC9 { get; set; }
        public int  MC10 { get; set; }
        public int MC11 { get; set; }
        public int MC12 { get; set; }
        public int MC13 { get; set; }
        public int MC14 { get; set; }
        public int MC15 { get; set; }
        public int MC16 { get; set; }
        public int MC17 { get; set; }
        public int MC18 { get; set; }
        public int MC19 { get; set; }
        public int MC20 { get; set; }
    }
}
