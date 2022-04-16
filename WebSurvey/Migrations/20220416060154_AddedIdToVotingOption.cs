using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSurvey.Migrations
{
    public partial class AddedIdToVotingOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VotingId",
                table: "VotingOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotingId",
                table: "VotingOptions");
        }
    }
}
