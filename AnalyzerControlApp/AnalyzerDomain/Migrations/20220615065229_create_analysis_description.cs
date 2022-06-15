using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnalyzerDomain.Migrations
{
    public partial class create_analysis_description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analyses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PatientId = table.Column<string>(nullable: true),
                    AnalysisType = table.Column<string>(nullable: true),
                    RotorPosition = table.Column<int>(nullable: false),
                    ConveyorPosition = table.Column<int>(nullable: false),
                    SampleVolume = table.Column<int>(nullable: false),
                    Tw2Volume = table.Column<int>(nullable: false),
                    Tw3Volume = table.Column<int>(nullable: false),
                    TacwVolume = table.Column<int>(nullable: false),
                    Inc1Duration = table.Column<int>(nullable: false),
                    inc2Duration = table.Column<int>(nullable: false),
                    CurrentStage = table.Column<int>(nullable: false),
                    IncubationStarted = table.Column<bool>(nullable: false),
                    RemainingIncubationTime = table.Column<int>(nullable: false),
                    OM1Value = table.Column<double>(nullable: false),
                    OM2Value = table.Column<double>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    Result = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analyses");
        }
    }
}
