using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class addworldcupgroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Stadium_StadiumId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_AwayTeamScore_TeamId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_HomeTeamScore_TeamId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_Match_MatchId",
                table: "Prediction");

            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_UserBetGroup_UserBetGroupId",
                table: "Prediction");

            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_Team_AwayTeamScore_TeamId",
                table: "Prediction");

            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_Team_HomeTeamScore_TeamId",
                table: "Prediction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBetGroup_BetGroup_BetGroupId",
                table: "UserBetGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBetGroup_AspNetUsers_UserId",
                table: "UserBetGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBetGroup",
                table: "UserBetGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stadium",
                table: "Stadium");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prediction",
                table: "Prediction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Match",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BetGroup",
                table: "BetGroup");

            migrationBuilder.RenameTable(
                name: "UserBetGroup",
                newName: "UserBetGroups");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "Stadium",
                newName: "Stadiums");

            migrationBuilder.RenameTable(
                name: "Prediction",
                newName: "Predictions");

            migrationBuilder.RenameTable(
                name: "Match",
                newName: "Matches");

            migrationBuilder.RenameTable(
                name: "BetGroup",
                newName: "BetGroups");

            migrationBuilder.RenameIndex(
                name: "IX_UserBetGroup_UserId",
                table: "UserBetGroups",
                newName: "IX_UserBetGroups_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBetGroup_BetGroupId",
                table: "UserBetGroups",
                newName: "IX_UserBetGroups_BetGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_HomeTeamScore_TeamId",
                table: "Predictions",
                newName: "IX_Predictions_HomeTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_AwayTeamScore_TeamId",
                table: "Predictions",
                newName: "IX_Predictions_AwayTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_UserBetGroupId",
                table: "Predictions",
                newName: "IX_Predictions_UserBetGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_MatchId",
                table: "Predictions",
                newName: "IX_Predictions_MatchId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_HomeTeamScore_TeamId",
                table: "Matches",
                newName: "IX_Matches_HomeTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_AwayTeamScore_TeamId",
                table: "Matches",
                newName: "IX_Matches_AwayTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_StadiumId",
                table: "Matches",
                newName: "IX_Matches_StadiumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBetGroups",
                table: "UserBetGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stadiums",
                table: "Stadiums",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Predictions",
                table: "Predictions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BetGroups",
                table: "BetGroups",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WorldCupGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldCupGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamWorldCupGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    WorldCupGroupId = table.Column<long>(nullable: false),
                    TeamId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamWorldCupGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamWorldCupGroups_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamWorldCupGroups_WorldCupGroups_WorldCupGroupId",
                        column: x => x.WorldCupGroupId,
                        principalTable: "WorldCupGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamWorldCupGroups_TeamId",
                table: "TeamWorldCupGroups",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamWorldCupGroups_WorldCupGroupId",
                table: "TeamWorldCupGroups",
                column: "WorldCupGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Stadiums_StadiumId",
                table: "Matches",
                column: "StadiumId",
                principalTable: "Stadiums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_AwayTeamScore_TeamId",
                table: "Matches",
                column: "AwayTeamScore_TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_HomeTeamScore_TeamId",
                table: "Matches",
                column: "HomeTeamScore_TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_Matches_MatchId",
                table: "Predictions",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_UserBetGroups_UserBetGroupId",
                table: "Predictions",
                column: "UserBetGroupId",
                principalTable: "UserBetGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_Teams_AwayTeamScore_TeamId",
                table: "Predictions",
                column: "AwayTeamScore_TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_Teams_HomeTeamScore_TeamId",
                table: "Predictions",
                column: "HomeTeamScore_TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBetGroups_BetGroups_BetGroupId",
                table: "UserBetGroups",
                column: "BetGroupId",
                principalTable: "BetGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBetGroups_AspNetUsers_UserId",
                table: "UserBetGroups",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Stadiums_StadiumId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_AwayTeamScore_TeamId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_HomeTeamScore_TeamId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_Matches_MatchId",
                table: "Predictions");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_UserBetGroups_UserBetGroupId",
                table: "Predictions");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_Teams_AwayTeamScore_TeamId",
                table: "Predictions");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_Teams_HomeTeamScore_TeamId",
                table: "Predictions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBetGroups_BetGroups_BetGroupId",
                table: "UserBetGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBetGroups_AspNetUsers_UserId",
                table: "UserBetGroups");

            migrationBuilder.DropTable(
                name: "TeamWorldCupGroups");

            migrationBuilder.DropTable(
                name: "WorldCupGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBetGroups",
                table: "UserBetGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stadiums",
                table: "Stadiums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Predictions",
                table: "Predictions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BetGroups",
                table: "BetGroups");

            migrationBuilder.RenameTable(
                name: "UserBetGroups",
                newName: "UserBetGroup");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.RenameTable(
                name: "Stadiums",
                newName: "Stadium");

            migrationBuilder.RenameTable(
                name: "Predictions",
                newName: "Prediction");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Match");

            migrationBuilder.RenameTable(
                name: "BetGroups",
                newName: "BetGroup");

            migrationBuilder.RenameIndex(
                name: "IX_UserBetGroups_UserId",
                table: "UserBetGroup",
                newName: "IX_UserBetGroup_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBetGroups_BetGroupId",
                table: "UserBetGroup",
                newName: "IX_UserBetGroup_BetGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_HomeTeamScore_TeamId",
                table: "Prediction",
                newName: "IX_Prediction_HomeTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_AwayTeamScore_TeamId",
                table: "Prediction",
                newName: "IX_Prediction_AwayTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_UserBetGroupId",
                table: "Prediction",
                newName: "IX_Prediction_UserBetGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_MatchId",
                table: "Prediction",
                newName: "IX_Prediction_MatchId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_HomeTeamScore_TeamId",
                table: "Match",
                newName: "IX_Match_HomeTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_AwayTeamScore_TeamId",
                table: "Match",
                newName: "IX_Match_AwayTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_StadiumId",
                table: "Match",
                newName: "IX_Match_StadiumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBetGroup",
                table: "UserBetGroup",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Team",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stadium",
                table: "Stadium",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prediction",
                table: "Prediction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Match",
                table: "Match",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BetGroup",
                table: "BetGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Stadium_StadiumId",
                table: "Match",
                column: "StadiumId",
                principalTable: "Stadium",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_AwayTeamScore_TeamId",
                table: "Match",
                column: "AwayTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_HomeTeamScore_TeamId",
                table: "Match",
                column: "HomeTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_Match_MatchId",
                table: "Prediction",
                column: "MatchId",
                principalTable: "Match",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_UserBetGroup_UserBetGroupId",
                table: "Prediction",
                column: "UserBetGroupId",
                principalTable: "UserBetGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_Team_AwayTeamScore_TeamId",
                table: "Prediction",
                column: "AwayTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_Team_HomeTeamScore_TeamId",
                table: "Prediction",
                column: "HomeTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBetGroup_BetGroup_BetGroupId",
                table: "UserBetGroup",
                column: "BetGroupId",
                principalTable: "BetGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBetGroup_AspNetUsers_UserId",
                table: "UserBetGroup",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
