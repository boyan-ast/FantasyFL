using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyFL.Data.Migrations
{
    public partial class RemovedFantasyTeamId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FantasyTeams_FantasyTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FantasyTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FantasyTeamId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FantasyTeamId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FantasyTeamId",
                table: "AspNetUsers",
                column: "FantasyTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FantasyTeams_FantasyTeamId",
                table: "AspNetUsers",
                column: "FantasyTeamId",
                principalTable: "FantasyTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
