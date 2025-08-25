using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderItemsAddedToDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Orders_OrderId",
                schema: "ordering",
                table: "OrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                schema: "ordering",
                table: "OrderItem");

            migrationBuilder.RenameTable(
                name: "OrderItem",
                schema: "ordering",
                newName: "OrderItems",
                newSchema: "ordering");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_OrderId",
                schema: "ordering",
                table: "OrderItems",
                newName: "IX_OrderItems_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                schema: "ordering",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "ordering",
                table: "OrderItems",
                column: "OrderId",
                principalSchema: "ordering",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "ordering",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                schema: "ordering",
                table: "OrderItems");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                schema: "ordering",
                newName: "OrderItem",
                newSchema: "ordering");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrderId",
                schema: "ordering",
                table: "OrderItem",
                newName: "IX_OrderItem_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                schema: "ordering",
                table: "OrderItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Orders_OrderId",
                schema: "ordering",
                table: "OrderItem",
                column: "OrderId",
                principalSchema: "ordering",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
