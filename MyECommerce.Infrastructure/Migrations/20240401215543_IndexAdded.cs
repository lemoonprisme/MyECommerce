using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IndexAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 3L, null, "ViewAllOrders", "VIEWALLORDERS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
