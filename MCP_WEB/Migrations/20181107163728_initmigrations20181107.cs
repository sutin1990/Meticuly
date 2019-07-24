using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MCP_WEB.Migrations
{
    public partial class initmigrations20181107 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Isonite",
                columns: table => new
                {
                    IsoniteCode = table.Column<string>(nullable: false),
                    BPCodeFrom = table.Column<string>(nullable: false),
                    BPCodeTO = table.Column<string>(nullable: false),
                    JigNo1 = table.Column<string>(nullable: true),
                    JigNo2 = table.Column<string>(nullable: true),
                    JigNo3 = table.Column<string>(nullable: true),
                    DocDate = table.Column<DateTime>(nullable: true),
                    DocStatus = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    IsoniteUserDef1 = table.Column<string>(nullable: true),
                    IsoniteUserDef2 = table.Column<string>(nullable: true),
                    IsoniteUserDef3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Isonite", x => x.IsoniteCode);
                });

            migrationBuilder.CreateTable(
                name: "Isonite_Line",
                columns: table => new
                {
                    RECID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsoniteCode = table.Column<string>(nullable: false),
                    JigNo = table.Column<string>(nullable: false),
                    JigAddresss = table.Column<int>(nullable: false),
                    BarCode = table.Column<string>(nullable: true),
                    SentQty = table.Column<int>(nullable: false),
                    RecQty = table.Column<int>(nullable: false),
                    RefIsoniteCode = table.Column<string>(nullable: true),
                    RefJigAddress = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<string>(nullable: true),
                    IsoniteLineUserDef1 = table.Column<string>(nullable: true),
                    IsoniteLineUserDef2 = table.Column<string>(nullable: true),
                    IsoniteLineUserDef3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Isonite_Line", x => x.RECID);
                });

            migrationBuilder.CreateTable(
                name: "isonite_temp",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BarCode = table.Column<string>(nullable: true),
                    Qty = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_isonite_temp", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "m_BOM",
                columns: table => new
                {
                    ItemCode = table.Column<string>(nullable: false),
                    Material1 = table.Column<string>(nullable: true),
                    Material2 = table.Column<string>(nullable: true),
                    BOMUserDef1 = table.Column<string>(nullable: true),
                    BOMUserDef2 = table.Column<string>(nullable: true),
                    BOMUserDef3 = table.Column<string>(nullable: true),
                    BOMUserDef4 = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_BOM", x => x.ItemCode);
                });

            migrationBuilder.CreateTable(
                name: "m_BPMaster",
                columns: table => new
                {
                    BPCode = table.Column<string>(nullable: false),
                    BPName = table.Column<string>(nullable: false),
                    BPType = table.Column<string>(nullable: false),
                    BPAddress1 = table.Column<string>(nullable: true),
                    BPAddress2 = table.Column<string>(nullable: true),
                    BPAddress3 = table.Column<string>(nullable: true),
                    BPAddress4 = table.Column<string>(nullable: true),
                    BPAddress5 = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    BPAddress6 = table.Column<string>(nullable: true),
                    TagFormat = table.Column<string>(nullable: true),
                    PackingID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_BPMaster", x => x.BPCode);
                });

            migrationBuilder.CreateTable(
                name: "m_Dep",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepID = table.Column<string>(nullable: false),
                    DepDesc = table.Column<string>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_Dep", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "m_Jig",
                columns: table => new
                {
                    JigID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JigNo = table.Column<string>(nullable: true),
                    Column = table.Column<int>(nullable: false),
                    Row = table.Column<int>(nullable: false),
                    JigUserDef1 = table.Column<string>(nullable: true),
                    JigUserDef2 = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_Jig", x => x.JigID);
                });

            migrationBuilder.CreateTable(
                name: "m_MachineMaster",
                columns: table => new
                {
                    MachineCode = table.Column<string>(nullable: false),
                    MachineName = table.Column<string>(nullable: false),
                    MachineUserDef1 = table.Column<string>(nullable: true),
                    MachineUserDef2 = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_MachineMaster", x => x.MachineCode);
                });

            migrationBuilder.CreateTable(
                name: "m_Package",
                columns: table => new
                {
                    PackID = table.Column<string>(nullable: false),
                    PackDesc = table.Column<string>(nullable: false),
                    PackType = table.Column<string>(nullable: false),
                    PackQty = table.Column<int>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_Package", x => x.PackID);
                });

            migrationBuilder.CreateTable(
                name: "m_ProcessMaster",
                columns: table => new
                {
                    ProcessCode = table.Column<string>(nullable: false),
                    ProcessName = table.Column<string>(nullable: false),
                    SysemProcessFLag = table.Column<string>(nullable: true),
                    AllowPartialFlag = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<string>(nullable: false),
                    SeqNo = table.Column<int>(nullable: true),
                    ProcessUserDef1 = table.Column<string>(nullable: true),
                    ProcessUserDef2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_ProcessMaster", x => x.ProcessCode);
                });

            migrationBuilder.CreateTable(
                name: "m_Resource",
                columns: table => new
                {
                    ItemCode = table.Column<string>(nullable: false),
                    ItemName = table.Column<string>(nullable: false),
                    Model = table.Column<string>(nullable: false),
                    Fcode = table.Column<string>(nullable: false),
                    BPCode = table.Column<string>(nullable: false),
                    ItemType = table.Column<string>(nullable: true),
                    StdLotSize = table.Column<int>(nullable: true),
                    Tolerance = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Dimension1 = table.Column<decimal>(nullable: true),
                    Dimension2 = table.Column<decimal>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Length = table.Column<int>(nullable: true),
                    Batch1 = table.Column<string>(nullable: true),
                    Batch2 = table.Column<string>(nullable: true),
                    Uom = table.Column<string>(nullable: true),
                    HeatNo = table.Column<string>(nullable: true),
                    LengthUom = table.Column<string>(nullable: true),
                    ItemUserDef1 = table.Column<string>(nullable: true),
                    ItemUserDef2 = table.Column<string>(nullable: true),
                    ItemUserDef3 = table.Column<string>(nullable: true),
                    ItemUserDef4 = table.Column<string>(nullable: true),
                    ItemUserDef5 = table.Column<string>(nullable: true),
                    ItemImage = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_Resource", x => x.ItemCode);
                });

            migrationBuilder.CreateTable(
                name: "m_Routing",
                columns: table => new
                {
                    ItemCode = table.Column<string>(nullable: false),
                    OperationNo = table.Column<int>(nullable: false),
                    ProcessCode = table.Column<string>(nullable: false),
                    MachineCode = table.Column<string>(nullable: true),
                    PiecePerMin = table.Column<decimal>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_Routing", x => new { x.ItemCode, x.OperationNo });
                });

            migrationBuilder.CreateTable(
                name: "m_Shift",
                columns: table => new
                {
                    ShiftID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ShiftType = table.Column<string>(nullable: false),
                    ShiftDesc = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_Shift", x => x.ShiftID);
                });

            migrationBuilder.CreateTable(
                name: "m_UserMaster",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    UserPassword = table.Column<string>(maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    UserEmail = table.Column<string>(nullable: false),
                    ClusterCode = table.Column<string>(nullable: false),
                    DepID = table.Column<string>(nullable: false),
                    ShiftID = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    EmployeeCode = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true),
                    LocationCode = table.Column<string>(nullable: true),
                    UserLocationId = table.Column<string>(nullable: true),
                    UserImage = table.Column<byte[]>(nullable: true),
                    UserExpireDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    UserCreated = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    LastSignedin = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    UserRoll = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_UserMaster", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "m_UserPermiss",
                columns: table => new
                {
                    PerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    MenuIdentity = table.Column<int>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_UserPermiss", x => x.PerID);
                });

            migrationBuilder.CreateTable(
                name: "MenuMaster",
                columns: table => new
                {
                    MenuIdentity = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MenuID = table.Column<string>(nullable: false),
                    MenuName = table.Column<string>(nullable: false),
                    Parent_MenuID = table.Column<string>(nullable: true),
                    User_Roll = table.Column<string>(nullable: false),
                    MenuFileName = table.Column<string>(nullable: true),
                    MenuURL = table.Column<string>(nullable: true),
                    USE_YN = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuMaster", x => x.MenuIdentity);
                });

            migrationBuilder.CreateTable(
                name: "s_GlobalPams",
                columns: table => new
                {
                    parm_key = table.Column<string>(nullable: false),
                    parm_desc = table.Column<string>(nullable: true),
                    opt1 = table.Column<string>(nullable: true),
                    opt2 = table.Column<string>(nullable: true),
                    param_value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_GlobalPams", x => x.parm_key);
                });

            migrationBuilder.CreateTable(
                name: "s_ProcessLog",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProcessID = table.Column<string>(nullable: false),
                    ProcessDate = table.Column<DateTime>(nullable: true),
                    ErrorKey = table.Column<int>(nullable: true),
                    RecordKey1 = table.Column<string>(nullable: true),
                    RecordKey2 = table.Column<string>(nullable: true),
                    Msg = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_ProcessLog", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(maxLength: 60, nullable: false),
                    UserPassword = table.Column<string>(maxLength: 60, nullable: false),
                    UserEmail = table.Column<string>(nullable: true),
                    ClusterId = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    LocationId = table.Column<string>(nullable: true),
                    UserLayer = table.Column<string>(nullable: true),
                    UserImage = table.Column<byte[]>(nullable: true),
                    EmployeeId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserExpireDate = table.Column<DateTime>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    UserCreated = table.Column<string>(nullable: true),
                    DateChanged = table.Column<DateTime>(nullable: true),
                    UserChanged = table.Column<string>(nullable: true),
                    UserLocationId = table.Column<string>(nullable: true),
                    PrintBillFlag = table.Column<string>(nullable: true),
                    MaxUserQty = table.Column<int>(nullable: true),
                    MaxAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyPlan",
                columns: table => new
                {
                    BarCode = table.Column<string>(nullable: false),
                    ItemCode = table.Column<string>(nullable: true),
                    QtyOrderAll = table.Column<int>(nullable: true),
                    QtyOrder = table.Column<int>(nullable: true),
                    SeriesLot = table.Column<string>(nullable: true),
                    StdLotSize = table.Column<int>(nullable: true),
                    WStatus = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(nullable: true),
                    UpdateBy = table.Column<string>(nullable: true),
                    PlanUserDef1 = table.Column<string>(nullable: true),
                    PlanUserDef2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyPlan", x => x.BarCode);
                });

            migrationBuilder.CreateTable(
                name: "WoBOM",
                columns: table => new
                {
                    BarCode = table.Column<string>(nullable: false),
                    Material1 = table.Column<string>(nullable: true),
                    Material2 = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WoBOM", x => x.BarCode);
                });

            migrationBuilder.CreateTable(
                name: "WoRouting",
                columns: table => new
                {
                    BarCode = table.Column<string>(nullable: false),
                    OperationNo = table.Column<int>(nullable: false),
                    ProcessCode = table.Column<string>(nullable: true),
                    MachineCode = table.Column<string>(nullable: true),
                    QtyOrder = table.Column<int>(nullable: true),
                    QtyComplete = table.Column<int>(nullable: true),
                    QtyNG = table.Column<int>(nullable: true),
                    AllowPartialFlag = table.Column<string>(nullable: true),
                    MainProcessFlag = table.Column<string>(nullable: true),
                    PStatus = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WoRouting", x => new { x.BarCode, x.OperationNo });
                });

            migrationBuilder.CreateTable(
                name: "WoRoutingMovement",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BarCode = table.Column<string>(nullable: true),
                    OperationNo = table.Column<int>(nullable: true),
                    ShiftID = table.Column<int>(nullable: true),
                    QtyComplete = table.Column<int>(nullable: true),
                    QtyNG = table.Column<int>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WoRoutingMovement", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_DepMenu",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepID = table.Column<string>(nullable: false),
                    MenuIdentity = table.Column<int>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_DepMenu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_m_DepMenu_MenuMaster_MenuIdentity",
                        column: x => x.MenuIdentity,
                        principalTable: "MenuMaster",
                        principalColumn: "MenuIdentity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_m_DepMenu_MenuIdentity",
                table: "m_DepMenu",
                column: "MenuIdentity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Isonite");

            migrationBuilder.DropTable(
                name: "Isonite_Line");

            migrationBuilder.DropTable(
                name: "isonite_temp");

            migrationBuilder.DropTable(
                name: "m_BOM");

            migrationBuilder.DropTable(
                name: "m_BPMaster");

            migrationBuilder.DropTable(
                name: "m_Dep");

            migrationBuilder.DropTable(
                name: "m_DepMenu");

            migrationBuilder.DropTable(
                name: "m_Jig");

            migrationBuilder.DropTable(
                name: "m_MachineMaster");

            migrationBuilder.DropTable(
                name: "m_Package");

            migrationBuilder.DropTable(
                name: "m_ProcessMaster");

            migrationBuilder.DropTable(
                name: "m_Resource");

            migrationBuilder.DropTable(
                name: "m_Routing");

            migrationBuilder.DropTable(
                name: "m_Shift");

            migrationBuilder.DropTable(
                name: "m_UserMaster");

            migrationBuilder.DropTable(
                name: "m_UserPermiss");

            migrationBuilder.DropTable(
                name: "s_GlobalPams");

            migrationBuilder.DropTable(
                name: "s_ProcessLog");

            migrationBuilder.DropTable(
                name: "UserMaster");

            migrationBuilder.DropTable(
                name: "WeeklyPlan");

            migrationBuilder.DropTable(
                name: "WoBOM");

            migrationBuilder.DropTable(
                name: "WoRouting");

            migrationBuilder.DropTable(
                name: "WoRoutingMovement");

            migrationBuilder.DropTable(
                name: "MenuMaster");
        }
    }
}
