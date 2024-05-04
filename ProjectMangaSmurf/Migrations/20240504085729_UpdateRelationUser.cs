using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMangaSmurf.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_Users",
                table: "KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_NhanVien",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_User_KhachHang",
                table: "KhachHang",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_User_NhanVien",
                table: "NhanViens",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_KhachHang",
                table: "KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_User_NhanVien",
                table: "NhanViens");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_Users",
                table: "KhachHang",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_NhanVien",
                table: "Users",
                column: "IdUser",
                principalTable: "NhanViens",
                principalColumn: "IdUser");
        }
    }
}
