using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class WeeklyPlan
    {
        [Key]
        [Required]
        public string BarCode { get; set; }
        public string ItemCode { get; set; }
     
        public string Fcode { get; set; }
        public string Model { get; set;}
        public int? QtyOrderAll { get; set; }
        public int? QtyOrder { get; set; }
        public string SeriesLot { get; set; }      

        public int? StdLotSize { get; set; }

        public string WStatus { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }

        public string PlanUserDef1 { get; set;}
        public string PlanUserDef2 { get; set; }

        public int? QtyCompleted { get; set; }//add field เพิ่ม เพื่อใช่ใน moveticket_lot # สุทิน 23/11/2018
        public int? QtyPacked { get; set; }

    }
}
