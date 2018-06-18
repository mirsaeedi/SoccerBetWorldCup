using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class addmatchstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchStatus",
                table: "Matches",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchStatus",
                table: "Matches");
        }
    }
}
