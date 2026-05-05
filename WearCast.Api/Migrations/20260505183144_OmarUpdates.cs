using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearCast.Api.Migrations
{
    /// <inheritdoc />
    public partial class OmarUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AverageRating",
                table: "FixedProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "FixedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalesCount",
                table: "FixedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FixedProductReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    FixedProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedProductReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedProductReviews_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FixedProductReviews_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FixedProductReviews_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedProductReviews_FixedProducts_FixedProductId",
                        column: x => x.FixedProductId,
                        principalTable: "FixedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM9SbQxmQZqU2tPdQKdWfY/qEJ7DHqXb5fsbjyEcKmP7JF0vZUVTNKjhjGJpGWG70Q==");

            migrationBuilder.CreateIndex(
                name: "IX_FixedProductReviews_CreatedById",
                table: "FixedProductReviews",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FixedProductReviews_CustomerId",
                table: "FixedProductReviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedProductReviews_FixedProductId_CustomerId",
                table: "FixedProductReviews",
                columns: new[] { "FixedProductId", "CustomerId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_FixedProductReviews_UpdatedById",
                table: "FixedProductReviews",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixedProductReviews");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "FixedProducts");

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "FixedProducts");

            migrationBuilder.DropColumn(
                name: "SalesCount",
                table: "FixedProducts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO6ay6RsAph7bOTx1gJ9bCbmUX+U2TcI7P/ih2sl4f5RAzXxkFdmEv8kh5AP3iSv+A==");
        }
    }
}
