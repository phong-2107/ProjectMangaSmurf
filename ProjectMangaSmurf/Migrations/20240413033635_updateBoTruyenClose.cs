using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMangaSmurf.Migrations
{
    /// <inheritdoc />
    public partial class updateBoTruyenClose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoaiTruyen_BoTruyen_BoTruyenIdBo",
                table: "LoaiTruyen");

            migrationBuilder.DropIndex(
                name: "IX_LoaiTruyen_BoTruyenIdBo",
                table: "LoaiTruyen");

            migrationBuilder.DropColumn(
                name: "BoTruyenIdBo",
                table: "LoaiTruyen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BoTruyenIdBo",
                table: "LoaiTruyen",
                type: "char(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoaiTruyen_BoTruyenIdBo",
                table: "LoaiTruyen",
                column: "BoTruyenIdBo");

            migrationBuilder.AddForeignKey(
                name: "FK_LoaiTruyen_BoTruyen_BoTruyenIdBo",
                table: "LoaiTruyen",
                column: "BoTruyenIdBo",
                principalTable: "BoTruyen",
                principalColumn: "id_bo");
        }
    }
}
