using Microsoft.EntityFrameworkCore.Migrations;

namespace AnalyzerDomain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalysisStage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    PipettingVolume = table.Column<int>(nullable: false),
                    NeedIncubation = table.Column<bool>(nullable: false),
                    IncubationTimeInMinutes = table.Column<int>(nullable: false),
                    NeedWashStep = table.Column<bool>(nullable: false),
                    NumberOfWashStep = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisStage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cartridges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    Barcode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartridges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    CartridgeId = table.Column<int>(nullable: true),
                    SamplingStageId = table.Column<int>(nullable: true),
                    ConjugateStageId = table.Column<int>(nullable: true),
                    EnzymeComplexStageId = table.Column<int>(nullable: true),
                    SubstrateStageId = table.Column<int>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_AnalysisTypes_AnalysisStage_ConjugateStageId",
                        column: x => x.ConjugateStageId,
                        principalTable: "AnalysisStage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisTypes_AnalysisStage_EnzymeComplexStageId",
                        column: x => x.EnzymeComplexStageId,
                        principalTable: "AnalysisStage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisTypes_AnalysisStage_SamplingStageId",
                        column: x => x.SamplingStageId,
                        principalTable: "AnalysisStage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisTypes_AnalysisStage_SubstrateStageId",
                        column: x => x.SubstrateStageId,
                        principalTable: "AnalysisStage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisTypes_CartridgeId",
                table: "AnalysisTypes",
                column: "CartridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisTypes_ConjugateStageId",
                table: "AnalysisTypes",
                column: "ConjugateStageId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisTypes_EnzymeComplexStageId",
                table: "AnalysisTypes",
                column: "EnzymeComplexStageId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisTypes_SamplingStageId",
                table: "AnalysisTypes",
                column: "SamplingStageId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisTypes_SubstrateStageId",
                table: "AnalysisTypes",
                column: "SubstrateStageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisTypes");

            migrationBuilder.DropTable(
                name: "Cartridges");

            migrationBuilder.DropTable(
                name: "AnalysisStage");
        }
    }
}
