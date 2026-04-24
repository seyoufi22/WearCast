using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearCast.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerDesignProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DressStyle",
                table: "DesignedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MainImageUrl",
                table: "DesignedProductColors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BackImageUrl",
                table: "CustomerDesigns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontImageUrl",
                table: "CustomerDesigns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeftImageUrl",
                table: "CustomerDesigns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RightImageUrl",
                table: "CustomerDesigns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKQHsKbIKJHnCXOfDe6w3Y5+IdooL9aMGGu3iWo1qBiJmeYJ910jdXu2g6MhX1V0TQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DressStyle",
                table: "DesignedProducts");

            migrationBuilder.DropColumn(
                name: "MainImageUrl",
                table: "DesignedProductColors");

            migrationBuilder.DropColumn(
                name: "BackImageUrl",
                table: "CustomerDesigns");

            migrationBuilder.DropColumn(
                name: "FrontImageUrl",
                table: "CustomerDesigns");

            migrationBuilder.DropColumn(
                name: "LeftImageUrl",
                table: "CustomerDesigns");

            migrationBuilder.DropColumn(
                name: "RightImageUrl",
                table: "CustomerDesigns");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDDeg0gBAE3otblzr/nlKWie6CaA0RdC9Pf5V/rPqWw5lY4gVTIHFtn3EQr8pl4Slg==");
        }
    }
}
