﻿// <auto-generated />
using System;
using MCP_WEB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MCP_WEB.Migrations
{
    [DbContext(typeof(NittanDBcontext))]
    [Migration("20180910035734_initailUpadte10092018")]
    partial class initailUpadte10092018
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MCP_WEB.Models.m_BOM", b =>
                {
                    b.Property<string>("PartNo")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BOMUserDef1");

                    b.Property<string>("BOMUserDef2");

                    b.Property<string>("BOMUserDef3");

                    b.Property<string>("BOMUserDef4");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Fcode");

                    b.Property<string>("Material1");

                    b.Property<string>("Material2");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("TransDate");

                    b.HasKey("PartNo");

                    b.ToTable("m_BOM");
                });

            modelBuilder.Entity("MCP_WEB.Models.m_BPMaster", b =>
                {
                    b.Property<string>("BPCode")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BPAddress1");

                    b.Property<string>("BPAddress2");

                    b.Property<string>("BPAddress3");

                    b.Property<string>("BPAddress4");

                    b.Property<string>("BPAddress5");

                    b.Property<string>("BPAddress6");

                    b.Property<string>("BPName")
                        .IsRequired();

                    b.Property<string>("BPType")
                        .IsRequired();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("PackingID");

                    b.Property<string>("TagFormat");

                    b.Property<DateTime?>("TransDate");

                    b.HasKey("BPCode");

                    b.ToTable("m_BPMaster");
                });

            modelBuilder.Entity("MCP_WEB.Models.m_Jig", b =>
                {
                    b.Property<int>("JigID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Column");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("JigNo");

                    b.Property<DateTime>("JigUserDef1");

                    b.Property<int>("JigUserDef2");

                    b.Property<string>("ModifyBy");

                    b.Property<int>("Row");

                    b.Property<DateTime>("TransDate");

                    b.HasKey("JigID");

                    b.ToTable("m_Jig");
                });

            modelBuilder.Entity("MCP_WEB.Models.m_MachineMaster", b =>
                {
                    b.Property<string>("MachineCode")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("MachineName")
                        .IsRequired();

                    b.Property<string>("MachineUserDef1");

                    b.Property<string>("MachineUserDef2");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("TransDate");

                    b.HasKey("MachineCode");

                    b.ToTable("m_MachineMaster");
                });

            modelBuilder.Entity("MCP_WEB.Models.m_Package", b =>
                {
                    b.Property<string>("PackID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("PackDesc")
                        .IsRequired();

                    b.Property<int>("PackQty");

                    b.Property<string>("PackType")
                        .IsRequired();

                    b.Property<DateTime>("TransDate");

                    b.HasKey("PackID");

                    b.ToTable("m_Package");
                });

            modelBuilder.Entity("MCP_WEB.Models.m_ProcessMaster", b =>
                {
                    b.Property<string>("ProcessCode")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("ProcessName");

                    b.Property<string>("SysemProcessFLag");

                    b.Property<DateTime?>("TransDate");

                    b.HasKey("ProcessCode");

                    b.ToTable("m_ProcessMaster");
                });

            modelBuilder.Entity("MCP_WEB.Models.m_Resource", b =>
                {
                    b.Property<string>("ItemCode")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BPCode")
                        .IsRequired();

                    b.Property<string>("Batch1");

                    b.Property<string>("Batch2");

                    b.Property<string>("Color");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<decimal?>("Dimension1");

                    b.Property<decimal?>("Dimension2");

                    b.Property<string>("Fcode")
                        .IsRequired();

                    b.Property<string>("HeatNo");

                    b.Property<string>("ItemName")
                        .IsRequired();

                    b.Property<string>("ItemType");

                    b.Property<string>("ItemUserDef1");

                    b.Property<string>("ItemUserDef2");

                    b.Property<string>("ItemUserDef3");

                    b.Property<string>("ItemUserDef4");

                    b.Property<string>("ItemUserDef5");

                    b.Property<int?>("Length");

                    b.Property<string>("LengthUom");

                    b.Property<string>("Model")
                        .IsRequired();

                    b.Property<string>("ModifyBy");

                    b.Property<string>("Status");

                    b.Property<int?>("StdLotSize");

                    b.Property<int?>("Tolerance");

                    b.Property<DateTime?>("TransDate");

                    b.Property<string>("Uom");

                    b.HasKey("ItemCode");

                    b.ToTable("m_Resource");
                });

            modelBuilder.Entity("MCP_WEB.Models.m_Routing", b =>
                {
                    b.Property<string>("FCode")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MachineCode");

                    b.Property<int?>("OperationNo")
                        .IsRequired();

                    b.Property<int?>("PiecePerMinStd");

                    b.Property<string>("ProcessCode")
                        .IsRequired();

                    b.HasKey("FCode");

                    b.ToTable("m_Routing");
                });

            modelBuilder.Entity("MCP_WEB.Models.m_Shift", b =>
                {
                    b.Property<int>("ShiftID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate");

                    b.Property<DateTime>("EndTime");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("ShiftDesc");

                    b.Property<string>("ShiftType")
                        .IsRequired();

                    b.Property<DateTime>("StartTime");

                    b.Property<DateTime>("TransDate");

                    b.HasKey("ShiftID");

                    b.ToTable("m_Shift");
                });

            modelBuilder.Entity("MCP_WEB.Models.MenuMaster", b =>
                {
                    b.Property<int>("MenuIdentity")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("MenuFileName");

                    b.Property<string>("MenuID")
                        .IsRequired();

                    b.Property<string>("MenuName")
                        .IsRequired();

                    b.Property<string>("MenuURL");

                    b.Property<string>("Parent_MenuID");

                    b.Property<string>("USE_YN");

                    b.Property<string>("User_Roll")
                        .IsRequired();

                    b.HasKey("MenuIdentity");

                    b.ToTable("MenuMaster");
                });

            modelBuilder.Entity("MCP_WEB.Models.UserMaster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClusterId");

                    b.Property<string>("CompanyId");

                    b.Property<DateTime?>("DateChanged");

                    b.Property<DateTime?>("DateCreated");

                    b.Property<string>("EmployeeId");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("LocationId");

                    b.Property<string>("MaxAddress");

                    b.Property<int?>("MaxUserQty");

                    b.Property<string>("PrintBillFlag");

                    b.Property<string>("UserChanged");

                    b.Property<string>("UserCreated");

                    b.Property<string>("UserEmail");

                    b.Property<DateTime?>("UserExpireDate");

                    b.Property<string>("UserId");

                    b.Property<byte[]>("UserImage");

                    b.Property<string>("UserLayer");

                    b.Property<string>("UserLocationId");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.HasKey("Id");

                    b.ToTable("UserMaster");
                });

            modelBuilder.Entity("MCP_WEB.Models.WeeklyPlan", b =>
                {
                    b.Property<string>("BarCode")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreateBy");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("FCode")
                        .IsRequired();

                    b.Property<string>("Model")
                        .IsRequired();

                    b.Property<int?>("QtyOrder");

                    b.Property<string>("SeriesLot");

                    b.Property<string>("UpdateBy");

                    b.Property<DateTime?>("UpdateDate");

                    b.Property<string>("WStatus");

                    b.HasKey("BarCode");

                    b.ToTable("WeeklyPlan");
                });
#pragma warning restore 612, 618
        }
    }
}
