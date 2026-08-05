using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileSync.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProtocolAndPortToCredential : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "Credentials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Protocol",
                table: "Credentials",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "Credentials");

            migrationBuilder.DropColumn(
                name: "Protocol",
                table: "Credentials");
        }
    }
}
