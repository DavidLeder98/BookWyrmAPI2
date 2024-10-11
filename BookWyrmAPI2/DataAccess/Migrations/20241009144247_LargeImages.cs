using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWyrmAPI2.Migrations
{
    /// <inheritdoc />
    public partial class LargeImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LargeImageUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                column: "LargeImageUrl",
                value: "LargeBrew.png");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                column: "LargeImageUrl",
                value: "LargeDragon.png");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                column: "LargeImageUrl",
                value: "LargeDeep.png");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4,
                column: "LargeImageUrl",
                value: "LargeAbj.png");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5,
                column: "LargeImageUrl",
                value: "LargeGold.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LargeImageUrl",
                table: "Books");
        }
    }
}
