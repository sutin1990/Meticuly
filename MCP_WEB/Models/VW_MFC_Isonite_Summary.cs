using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_Isonite_Summary
    {
        [Key]
        public string IsoniteCode { get; set; }
        public string JigNo { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public string BarCode { get; set; }
        public string SeriesLot { get; set; }
        public string Model { get; set; }
        public string WStatus { get; set; }
        public int QtyOrder { get; set; }
        public int WIPLeftQty { get; set; }
        public int IsoniteQty { get; set; }
    }
}
