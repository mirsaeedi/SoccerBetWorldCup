using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class add_prediction_rule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Predictions",
                table: "Predictions");

            migrationBuilder.RenameTable(
                name: "Predictions",
                newName: "MatchPredictions");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_HomeTeamScore_TeamId",
                table: "MatchPredictions",
                newName: "IX_MatchPredictions_HomeTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_AwayTeamScore_TeamId",
                table: "MatchPredictions",
                newName: "IX_MatchPredictions_AwayTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_UserBetGroupId",
                table: "MatchPredictions",
                newName: "IX_MatchPredictions_UserBetGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_MatchId",
                table: "MatchPredictions",
                newName: "IX_MatchPredictions_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchPredictions",
                table: "MatchPredictions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BetGroupBonusPredictionRules",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BetGroupId = table.Column<long>(nullable: false),
                    FirstTeamPredictionInGroupScore = table.Column<short>(nullable: false),
                    SecondTeamPredictionInGroupScore = table.Column<short>(nullable: false),
                    FirstTeamPredictionInWorldCupScore = table.Column<short>(nullable: false),
                    SecondTeamPredictionInWorldCupScore = table.Column<short>(nullable: false),
                    ThirdTeamPredictionInWorldCupScore = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetGroupBonusPredictionRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetGroupBonusPredictionRules_BetGroups_BetGroupId",
                        column: x => x.BetGroupId,
                        principalTable: "BetGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BetGroupMatchPredictionRules",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BetGroupId = table.Column<long>(nullable: false),
                    ExactMatchResultPredictionScore = table.Column<short>(nullable: false),
                    GoalDifferencePredictionScore = table.Column<short>(nullable: false),
                    WinnerPredictionScore = table.Column<short>(nullable: false),
                    PredictionScore = table.Column<short>(nullable: false),
                    MatchType = table.Column<int>(nullable: false),
                    MatchId = table.Column<long>(nullable: true),
                    ScoreCoefficient = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetGroupMatchPredictionRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetGroupMatchPredictionRules_BetGroups_BetGroupId",
                        column: x => x.BetGroupId,
                        principalTable: "BetGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BetGroupMatchPredictionRules_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BonusPredictions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MatchId = table.Column<long>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    WorldCupGoupId = table.Column<long>(nullable: false),
                    WorldCupGroupId = table.Column<long>(nullable: true),
                    UserBetGroupId = table.Column<long>(nullable: false),
                    BonusPredictionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusPredictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonusPredictions_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonusPredictions_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonusPredictions_UserBetGroups_UserBetGroupId",
                        column: x => x.UserBetGroupId,
                        principalTable: "UserBetGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonusPredictions_WorldCupGroups_WorldCupGroupId",
                        column: x => x.WorldCupGroupId,
                        principalTable: "WorldCupGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BetGroupBonusPredictionRules_BetGroupId",
                table: "BetGroupBonusPredictionRules",
                column: "BetGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BetGroupMatchPredictionRules_BetGroupId",
                table: "BetGroupMatchPredictionRules",
                column: "BetGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BetGroupMatchPredictionRules_MatchId",
                table: "BetGroupMatchPredictionRules",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusPredictions_MatchId",
                table: "BonusPredictions",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusPredictions_TeamId",
                table: "BonusPredictions",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusPredictions_UserBetGroupId",
                table: "BonusPredictions",
                column: "UserBetGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusPredictions_WorldCupGroupId",
                table: "BonusPredictions",
                column: "WorldCupGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPredictions_Matches_MatchId",
                table: "MatchPredictions",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPredictions_UserBetGroups_UserBetGroupId",
                table: "MatchPredictions",
                column: "UserBetGroupId",
                principalTable: "UserBetGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPredictions_Teams_AwayTeamScore_TeamId",
                table: "MatchPredictions",
                column: "AwayTeamScore_TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPredictions_Teams_HomeTeamScore_TeamId",
                table: "MatchPredictions",
                column: "HomeTeamScore_TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchPredictions_Matches_MatchId",
                table: "MatchPredictions");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPredictions_UserBetGroups_UserBetGroupId",
                table: "MatchPredictions");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPredictions_Teams_AwayTeamScore_TeamId",
                table: "MatchPredictions");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPredictions_Teams_HomeTeamScore_TeamId",
                table: "MatchPredictions");

            migrationBuilder.DropTable(
                name: "BetGroupBonusPredictionRules");

            migrationBuilder.DropTable(
                name: "BetGroupMatchPredictionRules");

            migrationBuilder.DropTable(
                name: "BonusPredictions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchPredictions",
                table: "MatchPredictions");

            migrationBuilder.RenameTable(
                name: "MatchPredictions",
                newName: "Predictions");

            migrationBuilder.RenameIndex(
                name: "IX_MatchPredictions_HomeTeamScore_TeamId",
                table: "Predictions",
                newName: "IX_Predictions_HomeTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchPredictions_AwayTeamScore_TeamId",
                table: "Predictions",
                newName: "IX_Predictions_AwayTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchPredictions_UserBetGroupId",
                table: "Predictions",
                newName: "IX_Predictions_UserBetGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchPredictions_MatchId",
                table: "Predictions",
                newName: "IX_Predictions_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Predictions",
                table: "Predictions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_Matches_MatchId",
                table: "Predictions",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
