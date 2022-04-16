using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSurvey.Migrations
{
    public partial class AddedVotingsToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                table: "Options");

            migrationBuilder.RenameTable(
                name: "Results",
                newName: "SurveyResults");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "SurveyQuestions");

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "SurveyQuestionOptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyResults",
                table: "SurveyResults",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyQuestions",
                table: "SurveyQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyQuestionOptions",
                table: "SurveyQuestionOptions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "VotingOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VotingResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VotingId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Votings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    IsPassworded = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VotingOptions");

            migrationBuilder.DropTable(
                name: "VotingResults");

            migrationBuilder.DropTable(
                name: "Votings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyResults",
                table: "SurveyResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyQuestions",
                table: "SurveyQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyQuestionOptions",
                table: "SurveyQuestionOptions");

            migrationBuilder.RenameTable(
                name: "SurveyResults",
                newName: "Results");

            migrationBuilder.RenameTable(
                name: "SurveyQuestions",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "SurveyQuestionOptions",
                newName: "Options");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Options",
                table: "Options",
                column: "Id");
        }
    }
}
