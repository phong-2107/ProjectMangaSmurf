using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMangaSmurf.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnToBoTruyen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Footer",
                columns: table => new
                {
                    link_fb = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    link_ins = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    link_x = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    link_discord = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    noidung = table.Column<string>(type: "text", nullable: true),
                    dieukhoan = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    giayphep = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    id_kh = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    ten_kh = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    sdt = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    lienket_fb = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    lienket_gg = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    taikhoan = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    matkhau = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    tt_premium = table.Column<bool>(type: "bit", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.id_kh);
                });

            migrationBuilder.CreateTable(
                name: "LoaiTruyen",
                columns: table => new
                {
                    id_loai = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    ten_loai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiTruyen", x => x.id_loai);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    id_nv = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    taikhoan = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    matkhau = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    loai_nv = table.Column<bool>(type: "bit", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.id_nv);
                });

            migrationBuilder.CreateTable(
                name: "Premium",
                columns: table => new
                {
                    gia_thanh = table.Column<decimal>(type: "decimal(6,0)", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TacGia",
                columns: table => new
                {
                    id_tg = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    ten_tg = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TacGia", x => x.id_tg);
                });

            migrationBuilder.CreateTable(
                name: "HopDong",
                columns: table => new
                {
                    id_hd = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: false),
                    ngaylap = table.Column<DateTime>(type: "datetime", nullable: false),
                    id_kh = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    gt_thanhtoan = table.Column<decimal>(type: "decimal(6,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopDong", x => x.id_hd);
                    table.ForeignKey(
                        name: "FK_HopDong_KhachHang",
                        column: x => x.id_kh,
                        principalTable: "KhachHang",
                        principalColumn: "id_kh");
                });

            migrationBuilder.CreateTable(
                name: "BoTruyen",
                columns: table => new
                {
                    id_bo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    ten_bo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    dotuoi = table.Column<byte>(type: "tinyint", nullable: false),
                    id_tg = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    id_loai = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    mota = table.Column<string>(type: "text", nullable: false),
                    tk_danhgia = table.Column<double>(type: "float", nullable: false),
                    tk_theodoi = table.Column<int>(type: "int", nullable: false),
                    TongLuotXem = table.Column<int>(type: "int", nullable: false),
                    anh_bia = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    anh_banner = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    tt_pemium = table.Column<bool>(type: "bit", nullable: false),
                    trang_thai = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoTruyen", x => x.id_bo);
                    table.ForeignKey(
                        name: "FK_BoTruyen_TacGia",
                        column: x => x.id_tg,
                        principalTable: "TacGia",
                        principalColumn: "id_tg");
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    id_bo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    stt_chap = table.Column<int>(type: "int", nullable: false),
                    ten_chap = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    thoi_gian = table.Column<DateTime>(type: "datetime", nullable: false),
                    tk_luotxem = table.Column<int>(type: "int", nullable: false),
                    tt_pemium = table.Column<bool>(type: "bit", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => new { x.stt_chap, x.id_bo });
                    table.ForeignKey(
                        name: "FK_Chapter_BoTruyen",
                        column: x => x.id_bo,
                        principalTable: "BoTruyen",
                        principalColumn: "id_bo");
                });

            migrationBuilder.CreateTable(
                name: "CT_BoTruyen",
                columns: table => new
                {
                    id_kh = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    ib_bo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    theodoi = table.Column<bool>(type: "bit", nullable: false),
                    danhgia = table.Column<byte>(name: "danh gia", type: "tinyint", nullable: false),
                    ls_moi = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_BoTruyen", x => new { x.id_kh, x.ib_bo });
                    table.ForeignKey(
                        name: "FK_CT_BoTruyen_BoTruyen",
                        column: x => x.ib_bo,
                        principalTable: "BoTruyen",
                        principalColumn: "id_bo");
                    table.ForeignKey(
                        name: "FK_CT_BoTruyen_KhachHang",
                        column: x => x.id_kh,
                        principalTable: "KhachHang",
                        principalColumn: "id_kh");
                });

            migrationBuilder.CreateTable(
                name: "CT_LoaiTruyen",
                columns: table => new
                {
                    id_loai = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    id_bo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    LoaiChinh = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_LoaiTruyen", x => new { x.id_loai, x.id_bo });
                    table.ForeignKey(
                        name: "FK_CT_LoaiTruyen_BoTruyen",
                        column: x => x.id_bo,
                        principalTable: "BoTruyen",
                        principalColumn: "id_bo");
                    table.ForeignKey(
                        name: "FK_CT_LoaiTruyen_LoaiTruyen",
                        column: x => x.id_loai,
                        principalTable: "LoaiTruyen",
                        principalColumn: "id_loai");
                });

            migrationBuilder.CreateTable(
                name: "CT_Chapter",
                columns: table => new
                {
                    so_trang = table.Column<int>(type: "int", nullable: false),
                    id_bo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    stt_chap = table.Column<int>(type: "int", nullable: false),
                    anh_trang = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_Chapter", x => new { x.so_trang, x.stt_chap, x.id_bo });
                    table.ForeignKey(
                        name: "FK_CT_Chapter_Chapter",
                        columns: x => new { x.stt_chap, x.id_bo },
                        principalTable: "Chapter",
                        principalColumns: new[] { "stt_chap", "id_bo" });
                });

            migrationBuilder.CreateTable(
                name: "CT_HoatDong",
                columns: table => new
                {
                    id_bo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    stt_chap = table.Column<int>(type: "int", nullable: false),
                    id_kh = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    tt_doc = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_HoatDong", x => new { x.id_kh, x.stt_chap, x.id_bo });
                    table.ForeignKey(
                        name: "FK_CT_HoatDong_Chapter",
                        columns: x => new { x.stt_chap, x.id_bo },
                        principalTable: "Chapter",
                        principalColumns: new[] { "stt_chap", "id_bo" });
                    table.ForeignKey(
                        name: "FK_CT_HoatDong_KhachHang",
                        column: x => x.id_kh,
                        principalTable: "KhachHang",
                        principalColumn: "id_kh");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoTruyen_id_tg",
                table: "BoTruyen",
                column: "id_tg");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_id_bo",
                table: "Chapter",
                column: "id_bo");

            migrationBuilder.CreateIndex(
                name: "IX_CT_BoTruyen_ib_bo",
                table: "CT_BoTruyen",
                column: "ib_bo");

            migrationBuilder.CreateIndex(
                name: "IX_CT_Chapter_stt_chap_id_bo",
                table: "CT_Chapter",
                columns: new[] { "stt_chap", "id_bo" });

            migrationBuilder.CreateIndex(
                name: "IX_CT_HoatDong_stt_chap_id_bo",
                table: "CT_HoatDong",
                columns: new[] { "stt_chap", "id_bo" });

            migrationBuilder.CreateIndex(
                name: "IX_CT_LoaiTruyen_id_bo",
                table: "CT_LoaiTruyen",
                column: "id_bo");

            migrationBuilder.CreateIndex(
                name: "IX_HopDong_id_kh",
                table: "HopDong",
                column: "id_kh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CT_BoTruyen");

            migrationBuilder.DropTable(
                name: "CT_Chapter");

            migrationBuilder.DropTable(
                name: "CT_HoatDong");

            migrationBuilder.DropTable(
                name: "CT_LoaiTruyen");

            migrationBuilder.DropTable(
                name: "Footer");

            migrationBuilder.DropTable(
                name: "HopDong");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "Premium");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "LoaiTruyen");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "BoTruyen");

            migrationBuilder.DropTable(
                name: "TacGia");
        }
    }
}
