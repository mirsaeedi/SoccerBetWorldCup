using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class add_winner_runnerup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RunnerupId",
                table: "WorldCupGroups",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WinnerId",
                table: "WorldCupGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorldCupGroups_RunnerupId",
                table: "WorldCupGroups",
                column: "RunnerupId");

            migrationBuilder.CreateIndex(
                name: "IX_WorldCupGroups_WinnerId",
                table: "WorldCupGroups",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorldCupGroups_Teams_RunnerupId",
                table: "WorldCupGroups",
                column: "RunnerupId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorldCupGroups_Teams_WinnerId",
                table: "WorldCupGroups",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorldCupGroups_Teams_RunnerupId",
                table: "WorldCupGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_WorldCupGroups_Teams_WinnerId",
                table: "WorldCupGroups");

            migrationBuilder.DropIndex(
                name: "IX_WorldCupGroups_RunnerupId",
                table: "WorldCupGroups");

            migrationBuilder.DropIndex(
                name: "IX_WorldCupGroups_WinnerId",
                table: "WorldCupGroups");

            migrationBuilder.DropColumn(
                name: "RunnerupId",
                table: "WorldCupGroups");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "WorldCupGroups");
        }
    }
}
