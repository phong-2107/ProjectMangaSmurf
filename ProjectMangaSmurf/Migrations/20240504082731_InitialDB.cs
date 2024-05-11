using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMangaSmurf.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Avatar",
                columns: table => new
                {
                    IdAvatar = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    AvatarContent = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Avatar__58BB37B073CA3349", x => x.IdAvatar);
                });

            migrationBuilder.CreateTable(
                name: "ContactMail",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMail", x => x.Email);
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
                name: "NhanViens",
                columns: table => new
                {
                    IdUser = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    StaffRole = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "PermissionsList",
                columns: table => new
                {
                    IdPermissions = table.Column<byte>(type: "tinyint", nullable: false),
                    Parent_Permissions = table.Column<byte>(type: "tinyint", nullable: true),
                    PermissionsName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PermissionsStats = table.Column<byte>(type: "tinyint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Permissi__0671E6D9E28B434E", x => x.IdPermissions);
                });

            migrationBuilder.CreateTable(
                name: "Service_Pack_Config",
                columns: table => new
                {
                    IdPack = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    Parent_Pack = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: true),
                    PackName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TicketSalary = table.Column<byte>(type: "tinyint", nullable: true),
                    ActivateTimeService = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(7,2)", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(7,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    PackActiveStats = table.Column<byte>(type: "tinyint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Service___FC84C5ABF37A7BFC", x => x.IdPack);
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
                name: "WebMedia_Config",
                columns: table => new
                {
                    IdConfig = table.Column<int>(type: "int", nullable: false),
                    ConfigValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WebMedia__79F21764D24494B8", x => x.IdConfig);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    Birth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<byte>(type: "tinyint", nullable: true),
                    TimeCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TimeUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__B7C92638206D968B", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_NhanVien",
                        column: x => x.IdUser,
                        principalTable: "NhanViens",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "Staff_Permissions_Detail",
                columns: table => new
                {
                    IdUser = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    IdPermissions = table.Column<byte>(type: "tinyint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Staff_Pe__37AE38558BE70804", x => new { x.IdUser, x.IdPermissions });
                    table.ForeignKey(
                        name: "FK_Staff_Permissions_Detail_NhanVien",
                        column: x => x.IdUser,
                        principalTable: "NhanViens",
                        principalColumn: "IdUser");
                    table.ForeignKey(
                        name: "FK_Staff_Permissions_Detail_PermissionsList",
                        column: x => x.IdPermissions,
                        principalTable: "PermissionsList",
                        principalColumn: "IdPermissions");
                });

            migrationBuilder.CreateTable(
                name: "BoTruyen",
                columns: table => new
                {
                    id_bo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    ten_bo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    dotuoi = table.Column<byte>(type: "tinyint", nullable: false),
                    id_tg = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    mota = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tk_danhgia = table.Column<double>(type: "float", nullable: false),
                    tk_theodoi = table.Column<int>(type: "int", nullable: false),
                    TongLuotXem = table.Column<int>(type: "int", nullable: false),
                    anh_bia = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    anh_banner = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    tt_pemium = table.Column<bool>(type: "bit", nullable: false),
                    trang_thai = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    listloai = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "KhachHang",
                columns: table => new
                {
                    IdUser = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    GoogleAccount = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    FacebookAccount = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IdAvatar = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    TicketSalary = table.Column<int>(type: "int", nullable: true),
                    Active_Premium = table.Column<bool>(type: "bit", nullable: true),
                    Active_Stats = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang_1", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_KhachHang_Avatar",
                        column: x => x.IdAvatar,
                        principalTable: "Avatar",
                        principalColumn: "IdAvatar");
                    table.ForeignKey(
                        name: "FK_KhachHang_Users",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "Staff_Active_Logs",
                columns: table => new
                {
                    IdLog = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: false),
                    IdUser = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    IdPermissions = table.Column<byte>(type: "tinyint", nullable: false),
                    ChangeDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TimeChanged = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff_Active_Logs_1", x => x.IdLog);
                    table.ForeignKey(
                        name: "FK_Staff_Active_Logs_Staff_Permissions_Detail",
                        columns: x => new { x.IdUser, x.IdPermissions },
                        principalTable: "Staff_Permissions_Detail",
                        principalColumns: new[] { "IdUser", "IdPermissions" });
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    id_bo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    stt_chap = table.Column<int>(type: "int", nullable: false),
                    ten_chap = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ChapterContent = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    thoi_gian = table.Column<DateTime>(type: "datetime", nullable: false),
                    TrangThai = table.Column<byte>(type: "tinyint", nullable: true),
                    tt_pemium = table.Column<bool>(type: "bit", nullable: false),
                    TicketCost = table.Column<byte>(type: "tinyint", nullable: true),
                    tk_luotxem = table.Column<int>(type: "int", nullable: false),
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
                name: "CT_LoaiTruyen",
                columns: table => new
                {
                    id_loai = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    id_bo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
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
                name: "CT_BoTruyen",
                columns: table => new
                {
                    IdUser = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    ib_bo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    theodoi = table.Column<bool>(type: "bit", nullable: false),
                    danhgia = table.Column<byte>(name: "danh gia", type: "tinyint", nullable: false),
                    ls_moi = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_BoTruyen", x => new { x.IdUser, x.ib_bo });
                    table.ForeignKey(
                        name: "FK_CT_BoTruyen_BoTruyen",
                        column: x => x.ib_bo,
                        principalTable: "BoTruyen",
                        principalColumn: "id_bo");
                    table.ForeignKey(
                        name: "FK_CT_BoTruyen_KhachHang",
                        column: x => x.IdUser,
                        principalTable: "KhachHang",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "Customer_Login",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: false),
                    IdUser = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__2B2C5B526B2A5304", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_Customer_Login_KhachHang",
                        column: x => x.IdUser,
                        principalTable: "KhachHang",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    IdPayment = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    IdUser = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    IdPack = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: true),
                    PayAmount = table.Column<decimal>(type: "decimal(7,2)", nullable: true),
                    PayMethod = table.Column<byte>(type: "tinyint", nullable: true),
                    PayStats = table.Column<byte>(type: "tinyint", nullable: true),
                    PayDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpiresTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__613289C0E7CD2C77", x => x.IdPayment);
                    
                    table.ForeignKey(
                        name: "FK_Payment_Service_Pack_Config",
                        column: x => x.IdPack,
                        principalTable: "Service_Pack_Config",
                        principalColumn: "IdPack");
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
                    IdUser = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    tt_doc = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_HoatDong", x => new { x.IdUser, x.stt_chap, x.id_bo });
                    table.ForeignKey(
                        name: "FK_CT_HoatDong_Chapter",
                        columns: x => new { x.stt_chap, x.id_bo },
                        principalTable: "Chapter",
                        principalColumns: new[] { "stt_chap", "id_bo" });
                    table.ForeignKey(
                        name: "FK_CT_HoatDong_KhachHang",
                        column: x => x.IdUser,
                        principalTable: "KhachHang",
                        principalColumn: "IdUser");
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
                name: "IX_Customer_Login_IdUser",
                table: "Customer_Login",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_IdAvatar",
                table: "KhachHang",
                column: "IdAvatar");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_IdPack",
                table: "Payment",
                column: "IdPack");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_Active_Logs_IdUser_IdPermissions",
                table: "Staff_Active_Logs",
                columns: new[] { "IdUser", "IdPermissions" });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_Permissions_Detail_IdPermissions",
                table: "Staff_Permissions_Detail",
                column: "IdPermissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactMail");

            migrationBuilder.DropTable(
                name: "CT_BoTruyen");

            migrationBuilder.DropTable(
                name: "CT_Chapter");

            migrationBuilder.DropTable(
                name: "CT_HoatDong");

            migrationBuilder.DropTable(
                name: "CT_LoaiTruyen");

            migrationBuilder.DropTable(
                name: "Customer_Login");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Staff_Active_Logs");

            migrationBuilder.DropTable(
                name: "WebMedia_Config");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "LoaiTruyen");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "Service_Pack_Config");

            migrationBuilder.DropTable(
                name: "Staff_Permissions_Detail");

            migrationBuilder.DropTable(
                name: "BoTruyen");

            migrationBuilder.DropTable(
                name: "Avatar");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PermissionsList");

            migrationBuilder.DropTable(
                name: "TacGia");

            migrationBuilder.DropTable(
                name: "NhanViens");
        }
    }
}
