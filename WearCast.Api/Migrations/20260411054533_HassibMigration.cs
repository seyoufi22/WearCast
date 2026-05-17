using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearCast.Api.Migrations
{
    /// <inheritdoc />
    public partial class HassibMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultColorId",
                table: "DesignedProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesCount",
                table: "DesignedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOAeht9P2DH7wsmAbsyKj3IV4ulEfMxdTjX6vNlHpVzVCxLb0eY4wDswGfc6RqXUTg==");

            migrationBuilder.CreateIndex(
                name: "IX_DesignedProducts_DefaultColorId",
                table: "DesignedProducts",
                column: "DefaultColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DesignedProducts_DesignedProductColors_DefaultColorId",
                table: "DesignedProducts",
                column: "DefaultColorId",
                principalTable: "DesignedProductColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignedProducts_DesignedProductColors_DefaultColorId",
                table: "DesignedProducts");

            migrationBuilder.DropIndex(
                name: "IX_DesignedProducts_DefaultColorId",
                table: "DesignedProducts");

            migrationBuilder.DropColumn(
                name: "DefaultColorId",
                table: "DesignedProducts");

            migrationBuilder.DropColumn(
                name: "SalesCount",
                table: "DesignedProducts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH/y5uMI1oSgpAGML4WizeaWyPMt6MCcPQpvm8OJrLpWuzlkjoY6PUL9TJ2ifh2BsQ==");
        }
    }
}
