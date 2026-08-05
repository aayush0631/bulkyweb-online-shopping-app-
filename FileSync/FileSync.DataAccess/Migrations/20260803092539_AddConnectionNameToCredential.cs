using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileSync.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionNameToCredential : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionName",
                table: "Credentials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionName",
                table: "Credentials");
        }
    }
}
