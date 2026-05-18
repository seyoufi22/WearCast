using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearCast.Api.Migrations
{
    /// <inheritdoc />
    public partial class MergeOroderToDevelopment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_UserId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_ShippingCompanies_ShippingCompanyId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingCompanyManagers_AspNetUsers_UserId",
                table: "ShippingCompanyManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingCompanyManagers_ShippingCompanies_ShippingCompanyId",
                table: "ShippingCompanyManagers");

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFee",
                table: "ShippingCompanies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryAddress_BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    ShipmentStatus = table.Column<int>(type: "int", nullable: false),
                    ShippingCompanyId = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shipments_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Shipments_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipments_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipments_ShippingCompanies_ShippingCompanyId",
                        column: x => x.ShippingCompanyId,
                        principalTable: "ShippingCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StripeSessionId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StripePaymentIntentId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RecipientName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RecipientPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RecipientAdditionalPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ShippingAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickUpAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickUpAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickUpAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickUpAddress_BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShipmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FixedProductOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    FixedColorId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ColorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
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
                    table.PrimaryKey("PK_FixedProductOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedProductOrderItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FixedProductOrderItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FixedProductOrderItems_FixedProductColors_FixedColorId",
                        column: x => x.FixedColorId,
                        principalTable: "FixedProductColors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedProductOrderItems_Orders_OrderId",
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
                value: "AQAAAAIAAYagAAAAEDDeg0gBAE3otblzr/nlKWie6CaA0RdC9Pf5V/rPqWw5lY4gVTIHFtn3EQr8pl4Slg==");

            migrationBuilder.CreateIndex(
                name: "IX_FixedProductOrderItems_CreatedById",
                table: "FixedProductOrderItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FixedProductOrderItems_FixedColorId",
                table: "FixedProductOrderItems",
                column: "FixedColorId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedProductOrderItems_OrderId",
                table: "FixedProductOrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedProductOrderItems_UpdatedById",
                table: "FixedProductOrderItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedById",
                table: "Orders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerId",
                table: "Orders",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipmentId",
                table: "Orders",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UpdatedById",
                table: "Orders",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_CreatedById",
                table: "Shipments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_CustomerID",
                table: "Shipments",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DriverId",
                table: "Shipments",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ShippingCompanyId",
                table: "Shipments",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_UpdatedById",
                table: "Shipments",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_UserId",
                table: "Drivers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_ShippingCompanies_ShippingCompanyId",
                table: "Drivers",
                column: "ShippingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingCompanyManagers_AspNetUsers_UserId",
                table: "ShippingCompanyManagers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingCompanyManagers_ShippingCompanies_ShippingCompanyId",
                table: "ShippingCompanyManagers",
                column: "ShippingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_UserId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_ShippingCompanies_ShippingCompanyId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingCompanyManagers_AspNetUsers_UserId",
                table: "ShippingCompanyManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingCompanyManagers_ShippingCompanies_ShippingCompanyId",
                table: "ShippingCompanyManagers");

            migrationBuilder.DropTable(
                name: "FixedProductOrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "ShippingCompanies");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDXFzSht6F9eU96yqK1zj1SSK/XPrQhT5TMIzRGDalaEplrjGBMeXmvtS48u7Pj0aQ==");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_UserId",
                table: "Drivers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_ShippingCompanies_ShippingCompanyId",
                table: "Drivers",
                column: "ShippingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingCompanyManagers_AspNetUsers_UserId",
                table: "ShippingCompanyManagers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingCompanyManagers_ShippingCompanies_ShippingCompanyId",
                table: "ShippingCompanyManagers",
                column: "ShippingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
