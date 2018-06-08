using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class Rename_Team_Result : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Team_FirstTeamScore_TeamId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Team_SecondTeamScore_TeamId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_Team_FirstTeamScore_TeamId",
                table: "Prediction");

            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_Team_SecondTeamScore_TeamId",
                table: "Prediction");

            migrationBuilder.RenameColumn(
                name: "SecondTeamScore_TeamId",
                table: "Prediction",
                newName: "HomeTeamScore_TeamId");

            migrationBuilder.RenameColumn(
                name: "FirstTeamScore_TeamId",
                table: "Prediction",
                newName: "AwayTeamScore_TeamId");

            migrationBuilder.RenameColumn(
                name: "SecondTeamScore_Score",
                table: "Prediction",
                newName: "HomeTeamScore_PenaltyResult");

            migrationBuilder.RenameColumn(
                name: "FirstTeamScore_Score",
                table: "Prediction",
                newName: "HomeTeamScore_GameResult");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_SecondTeamScore_TeamId",
                table: "Prediction",
                newName: "IX_Prediction_HomeTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_FirstTeamScore_TeamId",
                table: "Prediction",
                newName: "IX_Prediction_AwayTeamScore_TeamId");

            migrationBuilder.RenameColumn(
                name: "SecondTeamScore_TeamId",
                table: "Game",
                newName: "HomeTeamScore_TeamId");

            migrationBuilder.RenameColumn(
                name: "FirstTeamScore_TeamId",
                table: "Game",
                newName: "AwayTeamScore_TeamId");

            migrationBuilder.RenameColumn(
                name: "SecondTeamScore_Score",
                table: "Game",
                newName: "HomeTeamScore_PenaltyResult");

            migrationBuilder.RenameColumn(
                name: "FirstTeamScore_Score",
                table: "Game",
                newName: "HomeTeamScore_GameResult");

            migrationBuilder.RenameIndex(
                name: "IX_Game_SecondTeamScore_TeamId",
                table: "Game",
                newName: "IX_Game_HomeTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_FirstTeamScore_TeamId",
                table: "Game",
                newName: "IX_Game_AwayTeamScore_TeamId");

            migrationBuilder.AddColumn<string>(
                name: "FifaCode",
                table: "Team",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlagUrl",
                table: "Team",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "AwayTeamScore_GameResult",
                table: "Prediction",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "AwayTeamScore_PenaltyResult",
                table: "Prediction",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "AwayTeamScore_GameResult",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "AwayTeamScore_PenaltyResult",
                table: "Game",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Team_AwayTeamScore_TeamId",
                table: "Game",
                column: "AwayTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Team_HomeTeamScore_TeamId",
                table: "Game",
                column: "HomeTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_Team_AwayTeamScore_TeamId",
                table: "Prediction",
                column: "AwayTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_Team_HomeTeamScore_TeamId",
                table: "Prediction",
                column: "HomeTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Team_AwayTeamScore_TeamId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Team_HomeTeamScore_TeamId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_Team_AwayTeamScore_TeamId",
                table: "Prediction");

            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_Team_HomeTeamScore_TeamId",
                table: "Prediction");

            migrationBuilder.DropColumn(
                name: "FifaCode",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "FlagUrl",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "AwayTeamScore_GameResult",
                table: "Prediction");

            migrationBuilder.DropColumn(
                name: "AwayTeamScore_PenaltyResult",
                table: "Prediction");

            migrationBuilder.DropColumn(
                name: "AwayTeamScore_GameResult",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "AwayTeamScore_PenaltyResult",
                table: "Game");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore_TeamId",
                table: "Prediction",
                newName: "SecondTeamScore_TeamId");

            migrationBuilder.RenameColumn(
                name: "AwayTeamScore_TeamId",
                table: "Prediction",
                newName: "FirstTeamScore_TeamId");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore_PenaltyResult",
                table: "Prediction",
                newName: "SecondTeamScore_Score");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore_GameResult",
                table: "Prediction",
                newName: "FirstTeamScore_Score");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_HomeTeamScore_TeamId",
                table: "Prediction",
                newName: "IX_Prediction_SecondTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_AwayTeamScore_TeamId",
                table: "Prediction",
                newName: "IX_Prediction_FirstTeamScore_TeamId");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore_TeamId",
                table: "Game",
                newName: "SecondTeamScore_TeamId");

            migrationBuilder.RenameColumn(
                name: "AwayTeamScore_TeamId",
                table: "Game",
                newName: "FirstTeamScore_TeamId");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore_PenaltyResult",
                table: "Game",
                newName: "SecondTeamScore_Score");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore_GameResult",
                table: "Game",
                newName: "FirstTeamScore_Score");

            migrationBuilder.RenameIndex(
                name: "IX_Game_HomeTeamScore_TeamId",
                table: "Game",
                newName: "IX_Game_SecondTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_AwayTeamScore_TeamId",
                table: "Game",
                newName: "IX_Game_FirstTeamScore_TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Team_FirstTeamScore_TeamId",
                table: "Game",
                column: "FirstTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Team_SecondTeamScore_TeamId",
                table: "Game",
                column: "SecondTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_Team_FirstTeamScore_TeamId",
                table: "Prediction",
                column: "FirstTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_Team_SecondTeamScore_TeamId",
                table: "Prediction",
                column: "SecondTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
