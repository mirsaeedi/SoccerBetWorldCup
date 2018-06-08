using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class model_improvements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Team_AwayTeamScore_TeamId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Team_HomeTeamScore_TeamId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_Game_GameId",
                table: "Prediction");

            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_UserGroup_UserGroupId",
                table: "Prediction");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropIndex(
                name: "IX_Prediction_GameId",
                table: "Prediction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game",
                table: "Game");

            migrationBuilder.RenameTable(
                name: "Game",
                newName: "Match");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore_GameResult",
                table: "Prediction",
                newName: "HomeTeamScore_MatchResult");

            migrationBuilder.RenameColumn(
                name: "AwayTeamScore_GameResult",
                table: "Prediction",
                newName: "AwayTeamScore_MatchResult");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore_GameResult",
                table: "Match",
                newName: "HomeTeamScore_MatchResult");

            migrationBuilder.RenameColumn(
                name: "AwayTeamScore_GameResult",
                table: "Match",
                newName: "AwayTeamScore_MatchResult");

            migrationBuilder.RenameIndex(
                name: "IX_Game_HomeTeamScore_TeamId",
                table: "Match",
                newName: "IX_Match_HomeTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_AwayTeamScore_TeamId",
                table: "Match",
                newName: "IX_Match_AwayTeamScore_TeamId");

            migrationBuilder.AddColumn<long>(
                name: "MatchId",
                table: "Prediction",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatchType",
                table: "Match",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "StadiumId",
                table: "Match",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StaudiumId",
                table: "Match",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Match",
                table: "Match",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BetGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stadium",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stadium", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBetGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBetGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBetGroup_BetGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "BetGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBetGroup_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prediction_MatchId",
                table: "Prediction",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_StadiumId",
                table: "Match",
                column: "StadiumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBetGroup_GroupId",
                table: "UserBetGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBetGroup_UserId",
                table: "UserBetGroup",
                column: "UserId");

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
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_HomeTeamScore_TeamId",
                table: "Match",
                column: "HomeTeamScore_TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_Match_MatchId",
                table: "Prediction",
                column: "MatchId",
                principalTable: "Match",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_UserBetGroup_UserGroupId",
                table: "Prediction",
                column: "UserGroupId",
                principalTable: "UserBetGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "FK_Prediction_UserBetGroup_UserGroupId",
                table: "Prediction");

            migrationBuilder.DropTable(
                name: "Stadium");

            migrationBuilder.DropTable(
                name: "UserBetGroup");

            migrationBuilder.DropTable(
                name: "BetGroup");

            migrationBuilder.DropIndex(
                name: "IX_Prediction_MatchId",
                table: "Prediction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Match",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_StadiumId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Prediction");

            migrationBuilder.DropColumn(
                name: "MatchType",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "StadiumId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "StaudiumId",
                table: "Match");

            migrationBuilder.RenameTable(
                name: "Match",
                newName: "Game");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore_MatchResult",
                table: "Prediction",
                newName: "HomeTeamScore_GameResult");

            migrationBuilder.RenameColumn(
                name: "AwayTeamScore_MatchResult",
                table: "Prediction",
                newName: "AwayTeamScore_GameResult");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore_MatchResult",
                table: "Game",
                newName: "HomeTeamScore_GameResult");

            migrationBuilder.RenameColumn(
                name: "AwayTeamScore_MatchResult",
                table: "Game",
                newName: "AwayTeamScore_GameResult");

            migrationBuilder.RenameIndex(
                name: "IX_Match_HomeTeamScore_TeamId",
                table: "Game",
                newName: "IX_Game_HomeTeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_AwayTeamScore_TeamId",
                table: "Game",
                newName: "IX_Game_AwayTeamScore_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game",
                table: "Game",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroup_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prediction_GameId",
                table: "Prediction",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_GroupId",
                table: "UserGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_UserId",
                table: "UserGroup",
                column: "UserId");

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
                name: "FK_Prediction_Game_GameId",
                table: "Prediction",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_UserGroup_UserGroupId",
                table: "Prediction",
                column: "UserGroupId",
                principalTable: "UserGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
