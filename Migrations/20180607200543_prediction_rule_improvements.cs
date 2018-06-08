using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class prediction_rule_improvements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BonusPredictions_Matches_MatchId",
                table: "BonusPredictions");

            migrationBuilder.DropIndex(
                name: "IX_BonusPredictions_MatchId",
                table: "BonusPredictions");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "BonusPredictions");

            migrationBuilder.AlterColumn<int>(
                name: "MatchType",
                table: "BetGroupMatchPredictionRules",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<short>(
                name: "PenaltyPredictionScore",
                table: "BetGroupMatchPredictionRules",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<bool>(
                name: "UseFormulaForComputingScore",
                table: "BetGroupMatchPredictionRules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "WrongPredictionScore",
                table: "BetGroupMatchPredictionRules",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PenaltyPredictionScore",
                table: "BetGroupMatchPredictionRules");

            migrationBuilder.DropColumn(
                name: "UseFormulaForComputingScore",
                table: "BetGroupMatchPredictionRules");

            migrationBuilder.DropColumn(
                name: "WrongPredictionScore",
                table: "BetGroupMatchPredictionRules");

            migrationBuilder.AddColumn<long>(
                name: "MatchId",
                table: "BonusPredictions",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MatchType",
                table: "BetGroupMatchPredictionRules",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BonusPredictions_MatchId",
                table: "BonusPredictions",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_BonusPredictions_Matches_MatchId",
                table: "BonusPredictions",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
