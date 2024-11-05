using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWyrmAPI2.Migrations
{
    /// <inheritdoc />
    public partial class AppUserCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookIds",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookIds",
                table: "AspNetUsers");
        }
    }
}
