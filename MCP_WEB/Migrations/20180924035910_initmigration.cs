using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MCP_WEB.Migrations
{
    public partial class initmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FCode",
                table: "WeeklyPlan");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "WeeklyPlan");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "m_Jig");

            migrationBuilder.DropColumn(
                name: "JigUserDef2",
                table: "m_Jig");

            migrationBuilder.DropColumn(
                name: "TransDate",
                table: "m_Jig");

            migrationBuilder.AddColumn<string>(
                name: "ItemCode",
                table: "WeeklyPlan",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanUserDef1",
                table: "WeeklyPlan",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanUserDef2",
                table: "WeeklyPlan",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QtyOrderAll",
                table: "WeeklyPlan",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PiecePerMinStd",
                table: "m_Routing",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "m_Routing",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "m_Routing",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransDate",
                table: "m_Routing",
                nullable: true);

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
                    ModifyBy = table.Column<string>(nullable: true),
                    MenuMasterMenuIdentity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_Dep", x => x.ID);
                    table.ForeignKey(
                        name: "FK_m_Dep_MenuMaster_MenuMasterMenuIdentity",
                        column: x => x.MenuMasterMenuIdentity,
                        principalTable: "MenuMaster",
                        principalColumn: "MenuIdentity",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "m_DepMenu",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepID = table.Column<string>(nullable: false),
                    DepDesc = table.Column<string>(nullable: false),
                    MenuIdentity = table.Column<string>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<string>(nullable: true),
                    MenuMasterMenuIdentity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_DepMenu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_m_DepMenu_MenuMaster_MenuMasterMenuIdentity",
                        column: x => x.MenuMasterMenuIdentity,
                        principalTable: "MenuMaster",
                        principalColumn: "MenuIdentity",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "m_UserMaster",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    UserPassword = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    UserEmail = table.Column<string>(nullable: false),
                    ClusterCode = table.Column<string>(nullable: false),
                    DepID = table.Column<string>(nullable: true),
                    ShiftID = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: true),
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
                    ModifyBy = table.Column<string>(nullable: true)
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
                name: "s_ProcessLog",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProcessID = table.Column<string>(nullable: false),
                    ProcessDate = table.Column<DateTime>(nullable: true),
                    Msg = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_ProcessLog", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_m_Dep_MenuMasterMenuIdentity",
                table: "m_Dep",
                column: "MenuMasterMenuIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_m_DepMenu_MenuMasterMenuIdentity",
                table: "m_DepMenu",
                column: "MenuMasterMenuIdentity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_Dep");

            migrationBuilder.DropTable(
                name: "m_DepMenu");

            migrationBuilder.DropTable(
                name: "m_UserMaster");

            migrationBuilder.DropTable(
                name: "m_UserPermiss");

            migrationBuilder.DropTable(
                name: "s_ProcessLog");

            migrationBuilder.DropColumn(
                name: "ItemCode",
                table: "WeeklyPlan");

            migrationBuilder.DropColumn(
                name: "PlanUserDef1",
                table: "WeeklyPlan");

            migrationBuilder.DropColumn(
                name: "PlanUserDef2",
                table: "WeeklyPlan");

            migrationBuilder.DropColumn(
                name: "QtyOrderAll",
                table: "WeeklyPlan");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "m_Routing");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "m_Routing");

            migrationBuilder.DropColumn(
                name: "TransDate",
                table: "m_Routing");

            migrationBuilder.AddColumn<string>(
                name: "FCode",
                table: "WeeklyPlan",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "WeeklyPlan",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "PiecePerMinStd",
                table: "m_Routing",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "m_Jig",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "JigUserDef2",
                table: "m_Jig",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransDate",
                table: "m_Jig",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
