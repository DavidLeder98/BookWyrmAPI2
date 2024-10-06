using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookWyrmAPI2.Migrations
{
    /// <inheritdoc />
    public partial class BundleSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bundles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bundles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookBundles",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BundleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBundles", x => new { x.BookId, x.BundleId });
                    table.ForeignKey(
                        name: "FK_BookBundles_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookBundles_Bundles_BundleId",
                        column: x => x.BundleId,
                        principalTable: "Bundles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Bundles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Popular" },
                    { 2, "Best Sellers" },
                    { 3, "10% Off" }
                });

            migrationBuilder.InsertData(
                table: "BookBundles",
                columns: new[] { "BookId", "BundleId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 3 },
                    { 3, 1 },
                    { 3, 3 },
                    { 4, 3 },
                    { 5, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBundles_BundleId",
                table: "BookBundles",
                column: "BundleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBundles");

            migrationBuilder.DropTable(
                name: "Bundles");
        }
    }
}
