using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductToDbAndSeedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) 
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price50 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price100 = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Frank Herbert", "A sweeping science fiction epic set on the desert planet Arrakis, following Paul Atreides as he navigates politics, prophecy, and survival.", "9780441013593", 45m, 40m, 30m, 35m, "Dune" },
                    { 2, "Robert C. Martin", "A handbook of agile software craftsmanship, covering principles and practices for writing maintainable, readable code.", "9780132350884", 55m, 50m, 40m, 45m, "Clean Code" },
                    { 3, "J.R.R. Tolkien", "A reluctant hobbit, Bilbo Baggins, sets out on an unexpected journey to help a group of dwarves reclaim their homeland from a dragon.", "9780547928227", 35m, 30m, 24m, 27m, "The Hobbit" },
                    { 4, "James Clear", "A practical guide to building good habits and breaking bad ones, using small, incremental changes.", "9780735211292", 30m, 27m, 20m, 24m, "Atomic Habits" },
                    { 5, "George Orwell", "A dystopian novel depicting a totalitarian regime that enforces absolute control through surveillance and propaganda.", "9780451524935", 25m, 22m, 16m, 19m, "1984" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
