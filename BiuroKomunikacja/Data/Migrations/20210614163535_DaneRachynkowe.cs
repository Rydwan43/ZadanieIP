using Microsoft.EntityFrameworkCore.Migrations;

namespace BiuroKomunikacja.Data.Migrations
{
    public partial class DaneRachynkowe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DocsModel_ClientId",
                table: "DocsModel",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DocsModel_EmployeeId",
                table: "DocsModel",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocsModel_AspNetUsers_ClientId",
                table: "DocsModel",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocsModel_AspNetUsers_EmployeeId",
                table: "DocsModel",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocsModel_AspNetUsers_ClientId",
                table: "DocsModel");

            migrationBuilder.DropForeignKey(
                name: "FK_DocsModel_AspNetUsers_EmployeeId",
                table: "DocsModel");

            migrationBuilder.DropIndex(
                name: "IX_DocsModel_ClientId",
                table: "DocsModel");

            migrationBuilder.DropIndex(
                name: "IX_DocsModel_EmployeeId",
                table: "DocsModel");
        }
    }
}
