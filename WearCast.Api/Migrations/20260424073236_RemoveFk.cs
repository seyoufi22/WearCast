using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearCast.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDesignedOrderItems_CustomerDesigns_CustomerDesignId",
                table: "CustomerDesignedOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FixedProductOrderItems_FixedProductColors_FixedColorId",
                table: "FixedProductOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Customers_CustomerID",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_FixedProductOrderItems_FixedColorId",
                table: "FixedProductOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDesignedOrderItems_CustomerDesignId",
                table: "CustomerDesignedOrderItems");

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

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CustomerDesigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BackImageUrl",
                table: "CustomerDesignedOrderItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DesignedProductId",
                table: "CustomerDesignedOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FrontImageUrl",
                table: "CustomerDesignedOrderItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeftImageUrl",
                table: "CustomerDesignedOrderItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RightImageUrl",
                table: "CustomerDesignedOrderItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViewDesignsJson",
                table: "CustomerDesignedOrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe83a0d2-cc41-4305-bcea-799fe7af0de2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI6/MeFQPEJ6+hQBsCfG/lFu3cCb/0H6kyxVU1LZSpVNGQ1ULi0gn2xymjSQidP8CA==");

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

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CustomerDesigns");

            migrationBuilder.DropColumn(
                name: "BackImageUrl",
                table: "CustomerDesignedOrderItems");

            migrationBuilder.DropColumn(
                name: "DesignedProductId",
                table: "CustomerDesignedOrderItems");

            migrationBuilder.DropColumn(
                name: "FrontImageUrl",
                table: "CustomerDesignedOrderItems");

            migrationBuilder.DropColumn(
                name: "LeftImageUrl",
                table: "CustomerDesignedOrderItems");

            migrationBuilder.DropColumn(
                name: "RightImageUrl",
                table: "CustomerDesignedOrderItems");

            migrationBuilder.DropColumn(
                name: "ViewDesignsJson",
                table: "CustomerDesignedOrderItems");

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
                value: "AQAAAAIAAYagAAAAEI/Dz+yh2oLOQgGRoIeKPMVIfC5c31lzQJWDkn5iW1F4Fe1k4LYMjb8fmKActpVwUg==");

            migrationBuilder.CreateIndex(
                name: "IX_FixedProductOrderItems_FixedColorId",
                table: "FixedProductOrderItems",
                column: "FixedColorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDesignedOrderItems_CustomerDesignId",
                table: "CustomerDesignedOrderItems",
                column: "CustomerDesignId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDesignedOrderItems_CustomerDesigns_CustomerDesignId",
                table: "CustomerDesignedOrderItems",
                column: "CustomerDesignId",
                principalTable: "CustomerDesigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FixedProductOrderItems_FixedProductColors_FixedColorId",
                table: "FixedProductOrderItems",
                column: "FixedColorId",
                principalTable: "FixedProductColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
