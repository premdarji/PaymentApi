using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentAPI.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Cities_cityid",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "cityid",
                table: "Users",
                newName: "CityId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_cityid",
                table: "Users",
                newName: "IX_Users_CityId");

            migrationBuilder.RenameColumn(
                name: "CityID",
                table: "Cities",
                newName: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Cities_CityId",
                table: "Users",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Cities_CityId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Users",
                newName: "cityid");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CityId",
                table: "Users",
                newName: "IX_Users_cityid");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Cities",
                newName: "CityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Cities_cityid",
                table: "Users",
                column: "cityid",
                principalTable: "Cities",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
