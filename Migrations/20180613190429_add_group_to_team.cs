using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class add_group_to_team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WorldCupGroupId",
                table: "Teams",
                nullable: true,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_WorldCupGroupId",
                table: "Teams",
                column: "WorldCupGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_WorldCupGroups_WorldCupGroupId",
                table: "Teams",
                column: "WorldCupGroupId",
                principalTable: "WorldCupGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_WorldCupGroups_WorldCupGroupId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_WorldCupGroupId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "WorldCupGroupId",
                table: "Teams");
        }
    }
}
