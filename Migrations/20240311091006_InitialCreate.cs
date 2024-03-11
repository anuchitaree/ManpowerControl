using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ManpowerControl.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityNames",
                columns: table => new
                {
                    ActivityID = table.Column<string>(type: "text", nullable: false),
                    FactoryID = table.Column<string>(type: "text", nullable: false),
                    ActivityDetail = table.Column<string>(type: "text", nullable: false),
                    LineName = table.Column<string>(type: "text", nullable: true),
                    ProductModel = table.Column<string>(type: "text", nullable: true),
                    Pic = table.Column<string>(type: "text", nullable: true),
                    AutomationCategory = table.Column<string>(type: "text", nullable: true),
                    Feasibility = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    CategoryReasonIssue = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    SubCategoryDetail = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityNames", x => x.ActivityID);
                });

            migrationBuilder.CreateTable(
                name: "MhSavings",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityID = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    MhSavingPlan = table.Column<double>(type: "double precision", nullable: false),
                    MhSavingActual = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MhSavings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StepProgresses",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityID = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    StepProgressPlan = table.Column<int>(type: "integer", nullable: false),
                    StepProgressActual = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepProgresses", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityNames");

            migrationBuilder.DropTable(
                name: "MhSavings");

            migrationBuilder.DropTable(
                name: "StepProgresses");
        }
    }
}
