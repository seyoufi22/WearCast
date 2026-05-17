using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WearCast.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f92b7c4-8a15-4d29-9e81-2c7b5a193f4e", "b7e5c1a2-9d3f-482a-a1b4-5e9f8c2d7a31", false, false, "CatalogAdmin", "CATALOGADMIN" },
                    { "4c2b97f0-0a14-4869-b541-bc2961e5ef0a", "f19d273a-4a25-47e1-8812-70b3e5138f29", false, false, "VendorAdmin", "VENDORADMIN" },
                    { "8b1912a5-4dc8-4f24-850d-830238fba901", "e5d9c2e3-2e70-4f5a-a38f-a957b10284d7", false, false, "OperationsAdmin", "OPERATIONSADMIN" },
                    { "d8a2c1f5-3e7b-491a-82d4-5c9f1b7a2e38", "5d2e7a1b-9f3c-481a-a2b5-e6f9d8c7b1a2", false, false, "CustomerServiceAdmin", "CUSTOMERSERVICEADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFLnY6uxhzFmLBErOGGimNahM33HMY7KhReBW+HbqTG3QjUwzbRV1ERDhYgNzWTwTg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f92b7c4-8a15-4d29-9e81-2c7b5a193f4e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c2b97f0-0a14-4869-b541-bc2961e5ef0a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b1912a5-4dc8-4f24-850d-830238fba901");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d8a2c1f5-3e7b-491a-82d4-5c9f1b7a2e38");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGlHfAEDOuxEtRxngyIbFuoC3fzRmNtcEjk4O9hugYRknT0qgykM3reLNQJJlKEAog==");
        }
    }
}
