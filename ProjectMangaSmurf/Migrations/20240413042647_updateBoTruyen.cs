using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMangaSmurf.Migrations
{
    /// <inheritdoc />
    public partial class updateBoTruyen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "listloai",
                table: "BoTruyen",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "listloai",
                table: "BoTruyen");
        }
    }
}
