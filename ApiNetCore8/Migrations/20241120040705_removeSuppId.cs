using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiNetCore8.Migrations
{
    /// <inheritdoc />
    public partial class removeSuppId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Supplier_SupplierId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_SupplierId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_SupplierId",
                table: "Order",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Supplier_SupplierId",
                table: "Order",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
