using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMangaSmurf.Migrations
{
    /// <inheritdoc />
    public partial class BotruyenUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaiChinh",
                table: "CT_LoaiTruyen");

            migrationBuilder.DropColumn(
                name: "id_loai",
                table: "BoTruyen");

            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "CT_LoaiTruyen",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "mota",
                table: "BoTruyen",
                type: "ntext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                table: "CT_LoaiTruyen");

            migrationBuilder.AddColumn<string>(
                name: "LoaiChinh",
                table: "CT_LoaiTruyen",
                type: "char(5)",
                unicode: false,
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "mota",
                table: "BoTruyen",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ntext");

            migrationBuilder.AddColumn<string>(
                name: "id_loai",
                table: "BoTruyen",
                type: "char(5)",
                unicode: false,
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: "");
        }
    }
}
