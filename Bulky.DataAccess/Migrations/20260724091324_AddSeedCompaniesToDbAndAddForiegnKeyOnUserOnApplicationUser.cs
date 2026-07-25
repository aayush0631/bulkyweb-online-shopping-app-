using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedCompaniesToDbAndAddForiegnKeyOnUserOnApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "PostalCode", "StreetAddress", "phoneNumber", "state" },
                values: new object[,]
                {
                    { 1, "Kathmandu", "TechNova Solutions", "44600", "123 Main Street", 9811111111L, "Bagmati" },
                    { 2, "Pokhara", "Everest IT Pvt. Ltd.", "33700", "45 Lakeside Road", 9822222222L, "Gandaki" },
                    { 3, "Kathmandu", "Himalayan Traders", "44605", "78 Putalisadak", 9833333333L, "Bagmati" },
                    { 4, "Birtamode", "Future Soft Nepal", "57204", "12 Birtamode Chowk", 9844444444L, "Koshi" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
