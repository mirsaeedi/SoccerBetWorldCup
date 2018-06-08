using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class rename_to_betgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_UserBetGroup_UserGroupId",
                table: "Prediction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBetGroup_BetGroup_GroupId",
                table: "UserBetGroup");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "UserBetGroup",
                newName: "BetGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBetGroup_GroupId",
                table: "UserBetGroup",
                newName: "IX_UserBetGroup_BetGroupId");

            migrationBuilder.RenameColumn(
                name: "UserGroupId",
                table: "Prediction",
                newName: "UserBetGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_UserGroupId",
                table: "Prediction",
                newName: "IX_Prediction_UserBetGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_UserBetGroup_UserBetGroupId",
                table: "Prediction",
                column: "UserBetGroupId",
                principalTable: "UserBetGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBetGroup_BetGroup_BetGroupId",
                table: "UserBetGroup",
                column: "BetGroupId",
                principalTable: "BetGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_UserBetGroup_UserBetGroupId",
                table: "Prediction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBetGroup_BetGroup_BetGroupId",
                table: "UserBetGroup");

            migrationBuilder.RenameColumn(
                name: "BetGroupId",
                table: "UserBetGroup",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBetGroup_BetGroupId",
                table: "UserBetGroup",
                newName: "IX_UserBetGroup_GroupId");

            migrationBuilder.RenameColumn(
                name: "UserBetGroupId",
                table: "Prediction",
                newName: "UserGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_UserBetGroupId",
                table: "Prediction",
                newName: "IX_Prediction_UserGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_UserBetGroup_UserGroupId",
                table: "Prediction",
                column: "UserGroupId",
                principalTable: "UserBetGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBetGroup_BetGroup_GroupId",
                table: "UserBetGroup",
                column: "GroupId",
                principalTable: "BetGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
