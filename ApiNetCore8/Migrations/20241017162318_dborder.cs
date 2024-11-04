using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiNetCore8.Migrations
{
    /// <inheritdoc />
    public partial class dborder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderDetailName",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDetailName",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "OrderName",
                table: "Order");
        }
    }
}
