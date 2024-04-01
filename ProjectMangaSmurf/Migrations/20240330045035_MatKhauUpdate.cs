using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMangaSmurf.Migrations
{
    /// <inheritdoc />
    public partial class MatKhauUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "matkhau",
                table: "KhachHang",
                type: "nvarchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "matkhau",
                table: "KhachHang",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldUnicode: false);
        }
    }
}
