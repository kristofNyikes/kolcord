using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace kolcordWebApi.Migrations
{
    /// <inheritdoc />
    public partial class updateDefaultProfilePic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "bab51f15-17f2-4b08-8bc0-d9b6e425cf50", null, "User", "USER" },
                    { "bbd044c0-af33-42a2-8e3d-9b0de8379fd5", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bab51f15-17f2-4b08-8bc0-d9b6e425cf50");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbd044c0-af33-42a2-8e3d-9b0de8379fd5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6baf5bef-2227-4598-8dc1-1d5767ad687f", null, "User", "USER" },
                    { "e18bc51b-0f96-4e61-93ec-c53df9d776e9", null, "Admin", "ADMIN" }
                });
        }
    }
}
