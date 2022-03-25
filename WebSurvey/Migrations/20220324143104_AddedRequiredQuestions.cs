using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSurvey.Migrations
{
    public partial class AddedRequiredQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "Questions");
        }
    }
}
