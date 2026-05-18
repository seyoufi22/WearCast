using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearCast.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGlHfAEDOuxEtRxngyIbFuoC3fzRmNtcEjk4O9hugYRknT0qgykM3reLNQJJlKEAog==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI6/MeFQPEJ6+hQBsCfG/lFu3cCb/0H6kyxVU1LZSpVNGQ1ULi0gn2xymjSQidP8CA==");
        }
    }
}
