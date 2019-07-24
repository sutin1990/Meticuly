using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MCP_WEB.Migrations
{
    public partial class initailUpdate07092018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_BOM",
                columns: table => new
                {
                    PartNo = table.Column<string>(nullable: false),
                    Fcode = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_m_BOM", x => x.PartNo);
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
                name: "m_Jig",
                columns: table => new
                {
                    JigID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JigNo = table.Column<string>(nullable: true),
                    Column = table.Column<int>(nullable: false),
                    Row = table.Column<int>(nullable: false),
                    JigUserDef1 = table.Column<DateTime>(nullable: false),
                    JigUserDef2 = table.Column<int>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
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
                    TransDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
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
                    FCode = table.Column<string>(nullable: false),
                    OperationNo = table.Column<int>(nullable: false),
                    ProcessCode = table.Column<string>(nullable: false),
                    MachineCode = table.Column<string>(nullable: true),
                    PiecePerMinStd = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_Routing", x => x.FCode);
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
                    FCode = table.Column<string>(nullable: false),
                    Model = table.Column<string>(nullable: false),
                    QtyOrder = table.Column<int>(nullable: true),
                    SeriesLot = table.Column<string>(nullable: true),
                    WStatus = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(nullable: true),
                    UpdateBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyPlan", x => x.BarCode);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_BOM");

            migrationBuilder.DropTable(
                name: "m_BPMaster");

            migrationBuilder.DropTable(
                name: "m_Jig");

            migrationBuilder.DropTable(
                name: "m_MachineMaster");

            migrationBuilder.DropTable(
                name: "m_Package");

            migrationBuilder.DropTable(
                name: "m_Resource");

            migrationBuilder.DropTable(
                name: "m_Routing");

            migrationBuilder.DropTable(
                name: "m_Shift");

            migrationBuilder.DropTable(
                name: "MenuMaster");

            migrationBuilder.DropTable(
                name: "UserMaster");

            migrationBuilder.DropTable(
                name: "WeeklyPlan");
        }
    }
}
