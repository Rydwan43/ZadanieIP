using Microsoft.EntityFrameworkCore.Migrations;

namespace BiuroKomunikacja.Data.Migrations
{
    public partial class FixedDocs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DocsModel",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "DocsModel");
        }
    }
}
