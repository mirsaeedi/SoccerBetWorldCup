using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerBet.Migrations
{
    public partial class removeteamgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_WorldCupGroups_WorldCupGroupId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "TeamWorldCupGroups");

            migrationBuilder.AlterColumn<long>(
                name: "WorldCupGroupId",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_WorldCupGroups_WorldCupGroupId",
                table: "Teams",
                column: "WorldCupGroupId",
                principalTable: "WorldCupGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_WorldCupGroups_WorldCupGroupId",
                table: "Teams");

            migrationBuilder.AlterColumn<long>(
                name: "WorldCupGroupId",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TeamWorldCupGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<long>(nullable: false),
                    WorldCupGroupId = table.Column<long>(nullable: false)
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
                name: "FK_Teams_WorldCupGroups_WorldCupGroupId",
                table: "Teams",
                column: "WorldCupGroupId",
                principalTable: "WorldCupGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
