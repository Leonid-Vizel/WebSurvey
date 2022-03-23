using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSurvey.Migrations
{
    public partial class AddedSurveyParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "Questions",
                newName: "Type");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnonimous",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOneOff",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPassworded",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnonimous",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "IsOneOff",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "IsPassworded",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Surveys");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Questions",
                newName: "type");
        }
    }
}
