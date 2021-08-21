using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnalyzerDomain.Migrations
{
    public partial class AddSheduledAnalyzes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SheduledAnalyzes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Barcode = table.Column<string>(nullable: true),
                    AnalysisTypeId = table.Column<int>(nullable: true),
                    CurrentStage = table.Column<int>(nullable: false),
                    Result = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheduledAnalyzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SheduledAnalyzes_AnalysisTypes_AnalysisTypeId",
                        column: x => x.AnalysisTypeId,
                        principalTable: "AnalysisTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SheduledAnalyzes_AnalysisTypeId",
                table: "SheduledAnalyzes",
                column: "AnalysisTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SheduledAnalyzes");
        }
    }
}
