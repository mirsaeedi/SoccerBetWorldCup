using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class nullable_bonus_prediction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BonusPredictions_Matches_MatchId",
                table: "BonusPredictions");

            migrationBuilder.DropForeignKey(
                name: "FK_BonusPredictions_Teams_TeamId",
                table: "BonusPredictions");

            migrationBuilder.AlterColumn<long>(
                name: "WorldCupGoupId",
                table: "BonusPredictions",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "TeamId",
                table: "BonusPredictions",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "MatchId",
                table: "BonusPredictions",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_BonusPredictions_Matches_MatchId",
                table: "BonusPredictions",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BonusPredictions_Teams_TeamId",
                table: "BonusPredictions",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BonusPredictions_Matches_MatchId",
                table: "BonusPredictions");

            migrationBuilder.DropForeignKey(
                name: "FK_BonusPredictions_Teams_TeamId",
                table: "BonusPredictions");

            migrationBuilder.AlterColumn<long>(
                name: "WorldCupGoupId",
                table: "BonusPredictions",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TeamId",
                table: "BonusPredictions",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MatchId",
                table: "BonusPredictions",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BonusPredictions_Matches_MatchId",
                table: "BonusPredictions",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BonusPredictions_Teams_TeamId",
                table: "BonusPredictions",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
