using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthWebApi.Migrations
{
    public partial class removeTokenTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValidTokens");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb435b98-bd28-4a20-ab4a-62b124d9841b",
                column: "ConcurrencyStamp",
                value: "1a214b2b-5ef3-47c3-9954-a98fede9c766");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb467b98-bd28-6720-ab4a-645124d9834b",
                column: "ConcurrencyStamp",
                value: "34e64141-7d81-449a-9e86-cb6864726809");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "73dcc714-8fbc-41ac-a6af-756986ade684",
                column: "ConcurrencyStamp",
                value: "42f9f354-19bc-41ab-a977-6ac4443afb84");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fa435b98-bd28-4a20-8b4a-62b124d9841b",
                column: "ConcurrencyStamp",
                value: "d9acc76f-a88a-4496-b426-d8e4b336304b");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
