using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearCast.Api.Migrations
{
    /// <inheritdoc />
    public partial class addtracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Customers_CustomerID",
                table: "Shipments");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Shipments",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Shipments_CustomerID",
                table: "Shipments",
                newName: "IX_Shipments_CustomerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveredAt",
                table: "Shipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryCode",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OutForDeliveryAt",
                table: "Shipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadyForPickupAt",
                table: "Shipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TripStartedAt",
                table: "Shipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEJiLjoM+7EmJ+1JNqmDmkWpsnsoQ8TDtrYjcR659f+qsOaiZHcrnHhI3ZHybuiJBg==");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Customers_CustomerId",
                table: "Shipments",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Customers_CustomerId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DeliveredAt",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DeliveryCode",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "OutForDeliveryAt",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ReadyForPickupAt",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "TripStartedAt",
                table: "Shipments");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Shipments",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_Shipments_CustomerId",
                table: "Shipments",
                newName: "IX_Shipments_CustomerID");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOBhh7Su44TwRUGkYDIK/uMuntEy2Bx8MB9l5jkF+gP86HGvbu9Hx8w45RiK7nbvhA==");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Customers_CustomerID",
                table: "Shipments",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
