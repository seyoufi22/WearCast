using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearCast.Api.Migrations
{
    /// <inheritdoc />
    public partial class DesignedOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerDesignedOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CustomerDesignId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ColorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SizeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDesignedOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerDesignedOrderItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerDesignedOrderItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerDesignedOrderItems_CustomerDesigns_CustomerDesignId",
                        column: x => x.CustomerDesignId,
                        principalTable: "CustomerDesigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDesignedOrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO+qnwJEU9ZwLBlm91PZrSOHmCNmpA2o3eL00AjumaQn0BYRLmcORAXHyuRDm+WCTA==");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FactoryId",
                table: "Orders",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDesignedOrderItems_CreatedById",
                table: "CustomerDesignedOrderItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDesignedOrderItems_CustomerDesignId",
                table: "CustomerDesignedOrderItems",
                column: "CustomerDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDesignedOrderItems_OrderId",
                table: "CustomerDesignedOrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDesignedOrderItems_UpdatedById",
                table: "CustomerDesignedOrderItems",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Factories_FactoryId",
                table: "Orders",
                column: "FactoryId",
                principalTable: "Factories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Factories_FactoryId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "CustomerDesignedOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FactoryId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBywmgRikJZtwheCNQVi69IM9col9FyTkxCl7KPLcEaMCnjz9jt4SvwB2fS+rkLfTw==");
        }
    }
}
