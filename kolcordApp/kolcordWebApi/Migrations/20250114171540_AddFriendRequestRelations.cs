using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace kolcordWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFriendRequestRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4dd3bc5-728f-4f8e-9313-9e1421fcfde6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da658847-ad35-4952-a1c8-42de8afbb733");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6baf5bef-2227-4598-8dc1-1d5767ad687f", null, "User", "USER" },
                    { "e18bc51b-0f96-4e61-93ec-c53df9d776e9", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6baf5bef-2227-4598-8dc1-1d5767ad687f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e18bc51b-0f96-4e61-93ec-c53df9d776e9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a4dd3bc5-728f-4f8e-9313-9e1421fcfde6", null, "Admin", "ADMIN" },
                    { "da658847-ad35-4952-a1c8-42de8afbb733", null, "User", "USER" }
                });
        }
    }
}
