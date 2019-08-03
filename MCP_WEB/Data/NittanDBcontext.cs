using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Models;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace MCP_WEB.Data
{
    public class NittanDBcontext : DbContext
    {
        public NittanDBcontext(DbContextOptions<NittanDBcontext> options) : base(options)
        {

        }
        public virtual DbSet<VW_MFC_Header_Route_stage> VW_MFC_Header_Route_stage { get; set; }
        public virtual DbSet<VW_MFC_Route_stage> VW_MFC_Route_stage { get; set; }
        public virtual DbSet<VW_MFC_ROUTEINSPECT> VW_MFC_ROUTEINSPECT { get; set; }
        public virtual DbSet<VW_MFC_ROUTEINSPECT_SUB> VW_MFC_ROUTEINSPECT_SUB { get; set; }
        public virtual DbSet<VW_MFC_OSRN> VW_MFC_OSRN { get; set; }
        public virtual DbSet<VW_MFC_OWOR> VW_MFC_OWOR { get; set; }
        public virtual DbSet<VW_MFC_WOR4> VW_MFC_WOR4 { get; set; }
        public virtual DbSet<VW_MFC_M_INSPECT_RESULT> VW_MFC_M_INSPECT_RESULT { get; set; }
   
        public virtual DbSet<UserMaster> UserMaster { get; set; }
        public virtual DbSet<MenuMaster> MenuMaster { get; set; }
        public virtual DbSet<m_MachineMaster> m_MachineMaster { get; set; }
        public virtual DbSet<m_Resource> m_Resource { get; set; }
        public virtual DbSet<m_Package> m_Package { get; set; }
        public virtual DbSet<m_BPMaster> m_BPMaster { get; set; }
        public virtual DbSet<m_Shift> m_Shift { get; set; }
        public virtual DbSet<m_Routing> m_Routing { get; set; }
        public virtual DbSet<m_BOM> m_BOM { get; set; }
        public virtual DbSet<m_Jig> m_Jig { get; set; }
        public virtual DbSet<WeeklyPlan> WeeklyPlan { get; set; }
        public virtual DbSet<m_ProcessMaster> m_ProcessMaster { get; set; }
        public virtual DbSet<m_Dep> m_Dep { get; set; }

        public virtual DbSet<m_DepMenu> m_DepMenu { get; set; }
        public virtual DbSet<s_ProcessLog> s_ProcessLog { get; set; }
        public virtual DbSet<m_UserMaster> m_UserMaster { get; set; }
        public virtual DbSet<m_UserPermiss> m_UserPermiss { get; set; }
        public virtual DbSet<s_GlobalPams> s_GlobalPams { get; set; }

        public virtual DbSet<WoRouting> WoRouting { get; set; }
        public virtual DbSet<WoRoutingMovement> WoRoutingMovement { get; set; }
        public virtual DbSet<WoBOM> WoBOM { get; set; }

        public virtual DbSet<Isonite> Isonite { get; set; }
        public virtual DbSet<Isonite_Line> Isonite_Line { get; set; }

        public virtual DbSet<VW_MFC_T1HATCYamaha> VW_MFC_T1HATCYamaha { get; set; }
        public virtual DbSet<VW_MFC_T2_TR> VW_MFC_T2_TR { get; set; }
        public virtual DbSet<VW_MFC_T3_ZR> VW_MFC_T3_ZR { get; set; }
        public virtual DbSet<VW_MFC_T4_THM_GD_TSM> VW_MFC_T4_THM_GD_TSM { get; set; }
        public virtual DbSet<VW_MFC_T5_AAT> VW_MFC_T5_AAT { get; set; }

        public virtual DbSet<PartDescTagSelect> PartDescTagSelect { get; set; }
        public virtual DbSet<VW_MFC_PartDescTagSelect_Calculator> VW_MFC_PartDescTagSelect_Calculator { get; set; }
        public virtual DbSet<MoveTicket> MoveTicket { get; set; }
        public virtual DbSet<MoveTicket_LOT> MoveTicket_LOT { get; set; }
        public virtual DbSet<MoveTicketViewModel> MoveTicketViewModel { get; set; }
        public virtual DbSet<VW_MFC_MoveTicketVoid> VW_MFC_MoveTicketVoid { get; set; }
        public virtual DbSet<VW_MFC_RePrint_MoveTicket> VW_MFC_RePrint_MoveTicket { get; set; }


        public virtual DbSet<Isonite_temp> isonite_temp { get; set; }

        public virtual DbSet<m_NextNumber> m_NextNumber { get; set; }
        public virtual DbSet<UserTransaction> UserTransaction { get; set; }

        public virtual DbSet<VW_MFC_Isonite_Summary> VW_MFC_Isonite_Summary { get; set; }

        public virtual DbSet<VW_MFC_Deliverynote_Select> VW_MFC_Deliverynote_Select { get; set; }
        public virtual DbSet<DeliveryNote> DeliveryNote { get; set; }

        public virtual DbSet<VW_MFC_ReceiptbyDeliveryNote> VW_MFC_ReceiptbyDeliveryNote { get; set; }
        public virtual DbSet<VW_MFC_ReceiptbyMoveTicket> VW_MFC_ReceiptbyMoveTicket { get; set; }
        public virtual DbSet<VW_MFC_ReceiptbyMoveTicket_Detail> VW_MFC_ReceiptbyMoveTicket_Detail { get; set; }

        public virtual DbSet<m_Reason> m_Reason { get; set; }

        public virtual DbSet<VW_MFC_Isonite_Line> VW_MFC_Isonite_Line { get; set; }
        public virtual DbSet<VW_MFC_JigonIsonite> VW_MFC_JigonIsonite { get; set; }

        public virtual DbSet<VW_MFC_Dashboard1> VW_MFC_Dashboard1 { get; set; }

        public virtual DbSet<Isonite_Line_Temp> Isonite_Line_Temp { get; set; }

        public virtual DbSet<VW_MFC_ProductionDailyReport1> VW_MFC_ProductionDailyReport1 { get; set; }

        public virtual DbSet<VW_MFC_ProductionDailyReport2> VW_MFC_ProductionDailyReport2 { get; set; }

        public virtual DbSet<Log_Select_Print> Log_Select_Print { get; set; }

        public virtual DbSet<VW_MFC_UserMasters> VW_MFC_UserMasters { get; set; }

        public virtual DbSet<WipMovement> WipMovement { get; set; }
        public virtual DbSet<FGMovement> FGMovement { get; set; }
        public virtual DbSet<WIPProcessBalance> WIPProcessBalance { get; set; }
        public virtual DbSet<SummaryWIP> SummaryWIP { get; set; }
        public virtual DbSet<WIPStatus> WIPStatus { get; set; }
        public virtual DbSet<DCWIPbyProcess> DCWIPbyProcess { get; set; }
        public virtual DbSet<DCWIPbyProcessH> DCWIPbyProcessH { get; set; }
        public virtual DbSet<DCWIPbyShiftH> DCWIPbyShiftH { get; set; }
        public virtual DbSet<DailyReport> DailyReport { get; set; }

        public virtual DbSet<VW_MFC_WIPAdjust> VW_MFC_WIPAdjust { get; set; }

        public virtual DbSet<VW_MFC_BPMaster> VW_MFC_BPMaster { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<WoRouting>()
            .HasIndex(b => b.BarCode)
            .ForSqlServerIsClustered(false);

            modelBuilder.Entity<Isonite_temp>()
            .HasIndex(b => b.BarCode)
            .ForSqlServerIsClustered(false);

            modelBuilder.Entity<Isonite_Line>()
            .HasIndex(b => b.BarCode)
            .ForSqlServerIsClustered(false);

            modelBuilder.Entity<Isonite_Line_Temp>()
           .HasIndex(b => b.BarCode)
           .ForSqlServerIsClustered(false);

            modelBuilder.Entity<VW_MFC_T1HATCYamaha>()
            .HasKey(s => new { s.PartNo, s.QR });

            modelBuilder.Entity<VW_MFC_T2_TR>()
            .HasKey(s => new { s.PartNo, s.QR });

            modelBuilder.Entity<VW_MFC_T3_ZR>()
            .HasKey(s => new { s.PartNo, s.QR });

            modelBuilder.Entity<VW_MFC_T4_THM_GD_TSM>()
            .HasKey(s => new { s.PartNo, s.QR });

            modelBuilder.Entity<VW_MFC_T5_AAT>()
            .HasKey(s => new { s.PartNo, s.QR });

            modelBuilder.Entity<VW_MFC_Isonite_Summary>()
           .HasKey(s => new { s.IsoniteCode });

            modelBuilder.Entity<VW_MFC_Dashboard1>()
          .HasKey(s => new { s.ProcessCode });


            //modelBuilder.Entity<m_Jig>().HasKey(s => new { s.JigID, s.JigNo });
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Routing>().HasKey(s => new { s.ItemCode, s.OperationNo });
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WoRouting>().HasKey(s => new { s.BarCode, s.OperationNo });
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_NextNumber>().HasKey(s => new { s.FieldName, s.opt1, s.opt2 });
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_BOM>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_BOM>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_UserMaster>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_UserMaster>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_UserMaster>().Property(e => e.UserExpireDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_UserMaster>().Property(e => e.LastSignedin).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_UserMasters>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_UserMasters>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_UserMasters>().Property(e => e.UserExpireDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_UserMasters>().Property(e => e.LastSignedin).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuMaster>().Property(e => e.CreatedDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_MachineMaster>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_MachineMaster>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Resource>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Resource>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Package>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Package>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_BPMaster>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_BPMaster>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeliveryNote>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeliveryNote>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Isonite_Line>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Isonite_Line>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Isonite_Line>().Property(e => e.Sentdate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Isonite_Line>().Property(e => e.Receivedate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IsoniteSum>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IsoniteSum>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_BPMaster>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_BPMaster>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<m_Dep>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Dep>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_DepMenu>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_DepMenu>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Jig>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Jig>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_MachineMaster>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_MachineMaster>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Package>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Package>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_ProcessMaster>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_ProcessMaster>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Reason>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Reason>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Resource>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Resource>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Routing>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Routing>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Shift>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Shift>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Shift>().Property(e => e.StartTime).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_Shift>().Property(e => e.EndTime).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_UserPermiss>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<m_UserPermiss>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MoveTicket>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MoveTicket>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MoveTicket_LOT>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MoveTicket_LOT>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MoveTicketViewModel>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MoveTicketViewModel>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().Property(e => e.OrderDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReportingProcess>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReportingProcess>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<v_Routing>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<v_Routing>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_Isonite_Line>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_Isonite_Line>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_Isonite_Line>().Property(e => e.Sentdate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_Isonite_Line>().Property(e => e.Receivedate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_ReceiptbyMoveTicket_Detail>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_ReceiptbyMoveTicket_Detail>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_T1HATCYamaha>().Property(e => e.Date).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_T2_TR>().Property(e => e.DeliveryDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_T3_ZR>().Property(e => e.DeliveryDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_T4_THM_GD_TSM>().Property(e => e.Date0).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_T4_THM_GD_TSM>().Property(e => e.Date1).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_T4_THM_GD_TSM>().Property(e => e.Date2).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_T4_THM_GD_TSM>().Property(e => e.Date3).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_T4_THM_GD_TSM>().Property(e => e.Date4).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_T5_AAT>().Property(e => e.DeliveryDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WeeklyPlan>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WeeklyPlan>().Property(e => e.UpdateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WoBOM>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WoBOM>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WoRouting>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WoRouting>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WoRoutingMovement>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WoRoutingMovement>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WipMovement>().Property(e => e.Trdate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FGMovement>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WIPProcessBalance>().Property(e => e.Asof).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SummaryWIP>().Property(e => e.Asof).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WIPStatus>().Property(e => e.Asof).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VW_MFC_BPMaster>().Property(e => e.TransDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<VW_MFC_BPMaster>().Property(e => e.CreateDate).HasColumnType("datetime");
            base.OnModelCreating(modelBuilder);


        }

    }
}
