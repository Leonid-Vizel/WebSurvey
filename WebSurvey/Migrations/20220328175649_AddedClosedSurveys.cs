using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSurvey.Migrations
{
    public partial class AddedClosedSurveys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Surveys");
        }
    }
}
