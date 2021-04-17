using Microsoft.EntityFrameworkCore.Migrations;

namespace AnalyzerDomain.Migrations
{
    public partial class AddAnalysisTypesimple : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalysisTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    CartridgeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisTypes_Cartridges_CartridgeId",
                        column: x => x.CartridgeId,
                        principalTable: "Cartridges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisTypes_CartridgeId",
                table: "AnalysisTypes",
                column: "CartridgeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisTypes");
        }
    }
}
