using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MCP_WEB.Migrations
{
    public partial class initailUpadte10092018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TransDate",
                table: "m_MachineMaster",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "m_MachineMaster",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "m_ProcessMaster",
                columns: table => new
                {
                    ProcessCode = table.Column<string>(nullable: false),
                    ProcessName = table.Column<string>(nullable: true),
                    SysemProcessFLag = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_ProcessMaster", x => x.ProcessCode);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_ProcessMaster");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransDate",
                table: "m_MachineMaster",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "m_MachineMaster",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
