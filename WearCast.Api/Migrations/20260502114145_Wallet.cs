using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearCast.Api.Migrations
{
    /// <inheritdoc />
    public partial class Wallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "ShippingCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "Sellers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Commission",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Payout",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "Factories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlatformSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommissionPercentage = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformSettings", x => x.Id);
                    table.CheckConstraint("CK_PlatformSettings_Singleton", "[Id] = 1");
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OwnerType = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wallets_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceOrderId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEES/MzBIMLDDAXWObbtDuwZRg3ZhyyPRKAzRae7PjdJDabjl1g39vMP1zciYQdKJhQ==");

            migrationBuilder.InsertData(
                table: "PlatformSettings",
                columns: new[] { "Id", "CommissionPercentage", "UpdatedById", "UpdatedOn" },
                values: new object[] { 1, 2m, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingCompanies_WalletId",
                table: "ShippingCompanies",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_WalletId",
                table: "Sellers",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_WalletId",
                table: "Factories",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_WalletId",
                table: "Customers",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CreatedById",
                table: "Wallets",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UpdatedById",
                table: "Wallets",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_CreatedById",
                table: "WalletTransactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_UpdatedById",
                table: "WalletTransactions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId",
                table: "WalletTransactions",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Wallets_WalletId",
                table: "Customers",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Factories_Wallets_WalletId",
                table: "Factories",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sellers_Wallets_WalletId",
                table: "Sellers",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingCompanies_Wallets_WalletId",
                table: "ShippingCompanies",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Wallets_WalletId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Factories_Wallets_WalletId",
                table: "Factories");

            migrationBuilder.DropForeignKey(
                name: "FK_Sellers_Wallets_WalletId",
                table: "Sellers");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingCompanies_Wallets_WalletId",
                table: "ShippingCompanies");

            migrationBuilder.DropTable(
                name: "PlatformSettings");

            migrationBuilder.DropTable(
                name: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_ShippingCompanies_WalletId",
                table: "ShippingCompanies");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_WalletId",
                table: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Factories_WalletId",
                table: "Factories");

            migrationBuilder.DropIndex(
                name: "IX_Customers_WalletId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "ShippingCompanies");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "Commission",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Payout",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHMBoPz/T0pFXijLhnaJMAnlKitx5iTUu4pMYPZfn/exjPmixUmWDP7W1D+QtaIoLQ==");
        }
    }
}
