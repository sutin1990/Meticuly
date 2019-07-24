using Microsoft.EntityFrameworkCore.Migrations;

namespace MCP_WEB.Migrations
{
    public partial class initMigration20181108 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "jig_isonite_status",
                table: "m_Jig",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "jig_isonite_status",
                table: "m_Jig");
        }
    }
}
