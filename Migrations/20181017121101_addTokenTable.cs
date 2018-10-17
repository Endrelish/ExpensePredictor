using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthWebApi.Migrations
{
    public partial class addTokenTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValidTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb435b98-bd28-4a20-ab4a-62b124d9841b",
                column: "ConcurrencyStamp",
                value: "ae95948a-da99-4ca9-806d-bb8a5b00fd7a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb467b98-bd28-6720-ab4a-645124d9834b",
                column: "ConcurrencyStamp",
                value: "9f8628ea-1259-4e6c-ba30-534a4508ab44");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "73dcc714-8fbc-41ac-a6af-756986ade684",
                column: "ConcurrencyStamp",
                value: "6f577187-ded8-4c7f-8a00-db0840a4a29f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fa435b98-bd28-4a20-8b4a-62b124d9841b",
                column: "ConcurrencyStamp",
                value: "1870040a-8b43-45d3-8a02-d73c488af52b");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValidTokens");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb435b98-bd28-4a20-ab4a-62b124d9841b",
                column: "ConcurrencyStamp",
                value: "fc41cd81-8928-48ba-a110-fccd66ec5127");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb467b98-bd28-6720-ab4a-645124d9834b",
                column: "ConcurrencyStamp",
                value: "bd8e1147-5ce3-421b-8621-12f24454c7a1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "73dcc714-8fbc-41ac-a6af-756986ade684",
                column: "ConcurrencyStamp",
                value: "e49acff7-6c9e-4ff2-ba43-2873e4505935");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fa435b98-bd28-4a20-8b4a-62b124d9841b",
                column: "ConcurrencyStamp",
                value: "69581dbd-ae21-4c7e-a1d7-c816701bdf1e");
        }
    }
}
