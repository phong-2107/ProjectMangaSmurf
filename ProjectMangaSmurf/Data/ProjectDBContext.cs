using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Models;
using System.Reflection.Emit;

namespace ProjectMangaSmurf.Data
{
    public class ProjectDBContext : DbContext
    {
        public ProjectDBContext(DbContextOptions<ProjectDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Avatar> Avatars { get; set; }

        public virtual DbSet<BoTruyen> BoTruyens { get; set; }

        public virtual DbSet<Chapter> Chapters { get; set; }

        public virtual DbSet<ContactMail> ContactMails { get; set; }

        public virtual DbSet<CtBoTruyen> CtBoTruyens { get; set; }

        public virtual DbSet<CtChapter> CtChapters { get; set; }

        public virtual DbSet<CtHoatDong> CtHoatDongs { get; set; }

        public virtual DbSet<CtLoaiTruyen> CtLoaiTruyens { get; set; }

        public virtual DbSet<CustomerLogin> CustomerLogins { get; set; }

        public virtual DbSet<KhachHang> KhachHangs { get; set; }

        public virtual DbSet<LoaiTruyen> LoaiTruyens { get; set; }

        public virtual DbSet<NhanVien> NhanViens { get; set; }

        public virtual DbSet<Payment> Payments { get; set; }

        public virtual DbSet<PermissionsList> PermissionsLists { get; set; }

        public virtual DbSet<ServicePackConfig> ServicePackConfigs { get; set; }

        public virtual DbSet<StaffActiveLog> StaffActiveLogs { get; set; }

        public virtual DbSet<StaffPermissionsDetail> StaffPermissionsDetails { get; set; }

        public virtual DbSet<TacGium> TacGia { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<WebMediaConfig> WebMediaConfigs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Avatar>(entity =>
            {
                entity.HasKey(e => e.IdAvatar).HasName("PK__Avatar__58BB37B073CA3349");

                entity.ToTable("Avatar");

                entity.Property(e => e.IdAvatar)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.AvatarContent).IsUnicode(false);
            });

            modelBuilder.Entity<BoTruyen>(entity =>
            {
                entity.HasKey(e => e.IdBo);

                entity.ToTable("BoTruyen");

                entity.HasIndex(e => e.IdTg, "IX_BoTruyen_id_tg");

                entity.Property(e => e.IdBo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_bo");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.AnhBanner)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("anh_banner");
                entity.Property(e => e.AnhBia)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("anh_bia");
                entity.Property(e => e.Dotuoi).HasColumnName("dotuoi");
                entity.Property(e => e.IdTg)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_tg");
                entity.Property(e => e.Listloai).HasColumnName("listloai");
                entity.Property(e => e.Mota).HasColumnName("mota");
                entity.Property(e => e.TenBo)
                    .HasMaxLength(30)
                    .HasColumnName("ten_bo");
                entity.Property(e => e.TkDanhgia).HasColumnName("tk_danhgia");
                entity.Property(e => e.TkTheodoi).HasColumnName("tk_theodoi");
                entity.Property(e => e.TrangThai).HasColumnName("trang_thai");
                entity.Property(e => e.TtPemium).HasColumnName("tt_pemium");

                entity.HasOne(d => d.IdTgNavigation).WithMany(p => p.BoTruyens)
                    .HasForeignKey(d => d.IdTg)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoTruyen_TacGia");
            });

            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.HasKey(e => new { e.SttChap, e.IdBo });

                entity.ToTable("Chapter");

                entity.HasIndex(e => e.IdBo, "IX_Chapter_id_bo");

                entity.Property(e => e.SttChap).HasColumnName("stt_chap");
                entity.Property(e => e.IdBo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_bo");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.ChapterContent).IsUnicode(false);
                entity.Property(e => e.TenChap)
                    .HasMaxLength(30)
                    .HasColumnName("ten_chap");
                entity.Property(e => e.ThoiGian)
                    .HasColumnType("datetime")
                    .HasColumnName("thoi_gian");
                entity.Property(e => e.TkLuotxem).HasColumnName("tk_luotxem");
                entity.Property(e => e.TtPemium).HasColumnName("tt_pemium");

                entity.HasOne(d => d.IdBoNavigation).WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.IdBo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chapter_BoTruyen");
            });

            modelBuilder.Entity<ContactMail>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.ToTable("ContactMail");
            });

            modelBuilder.Entity<CtBoTruyen>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IbBo });

                entity.ToTable("CT_BoTruyen");

                entity.HasIndex(e => e.IbBo, "IX_CT_BoTruyen_ib_bo");

                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.IbBo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("ib_bo");
                entity.Property(e => e.DanhGia).HasColumnName("danh gia");
                entity.Property(e => e.LsMoi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ls_moi");
                entity.Property(e => e.Theodoi).HasColumnName("theodoi");

                entity.HasOne(d => d.IbBoNavigation).WithMany(p => p.CtBoTruyens)
                    .HasForeignKey(d => d.IbBo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_BoTruyen_BoTruyen");

                entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.CtBoTruyens)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_BoTruyen_KhachHang");
            });

            modelBuilder.Entity<CtChapter>(entity =>
            {
                entity.HasKey(e => new { e.SoTrang, e.SttChap, e.IdBo });

                entity.ToTable("CT_Chapter");

                entity.HasIndex(e => new { e.SttChap, e.IdBo }, "IX_CT_Chapter_stt_chap_id_bo");

                entity.Property(e => e.SoTrang).HasColumnName("so_trang");
                entity.Property(e => e.SttChap).HasColumnName("stt_chap");
                entity.Property(e => e.IdBo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_bo");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.AnhTrang)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("anh_trang");

                entity.HasOne(d => d.Chapter).WithMany(p => p.CtChapters)
                    .HasForeignKey(d => new { d.SttChap, d.IdBo })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_Chapter_Chapter");
            });

            modelBuilder.Entity<CtHoatDong>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.SttChap, e.IdBo });

                entity.ToTable("CT_HoatDong");

                entity.HasIndex(e => new { e.SttChap, e.IdBo }, "IX_CT_HoatDong_stt_chap_id_bo");

                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.SttChap).HasColumnName("stt_chap");
                entity.Property(e => e.IdBo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_bo");
                entity.Property(e => e.TtDoc).HasColumnName("tt_doc");

                entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.CtHoatDongs)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_HoatDong_KhachHang");

                entity.HasOne(d => d.Chapter).WithMany(p => p.CtHoatDongs)
                    .HasForeignKey(d => new { d.SttChap, d.IdBo })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_HoatDong_Chapter");
            });

            modelBuilder.Entity<CtLoaiTruyen>(entity =>
            {
                entity.HasKey(e => new { e.IdLoai, e.IdBo });

                entity.ToTable("CT_LoaiTruyen");

                entity.HasIndex(e => e.IdBo, "IX_CT_LoaiTruyen_id_bo");

                entity.Property(e => e.IdLoai)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_loai");
                entity.Property(e => e.IdBo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_bo");
                entity.Property(e => e.Active).HasColumnName("active");

                entity.HasOne(d => d.IdBoNavigation).WithMany(p => p.CtLoaiTruyens)
                    .HasForeignKey(d => d.IdBo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_LoaiTruyen_BoTruyen");

                entity.HasOne(d => d.IdLoaiNavigation).WithMany(p => p.CtLoaiTruyens)
                    .HasForeignKey(d => d.IdLoai)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_LoaiTruyen_LoaiTruyen");
            });

            modelBuilder.Entity<CustomerLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey }).HasName("PK__Customer__2B2C5B526B2A5304");

                entity.ToTable("Customer_Login");

                entity.Property(e => e.LoginProvider).HasMaxLength(225);
                entity.Property(e => e.ProviderKey).HasMaxLength(225);
                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.CustomerLogins)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Customer_Login_KhachHang");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.IdUser).HasName("PK_KhachHang_1");

                entity.ToTable("KhachHang");

                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.ActivePremium).HasColumnName("Active_Premium");
                entity.Property(e => e.ActiveStats).HasColumnName("Active_Stats");
                entity.Property(e => e.FacebookAccount).IsUnicode(false);
                entity.Property(e => e.GoogleAccount).IsUnicode(false);
                entity.Property(e => e.IdAvatar)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdAvatarNavigation).WithMany(p => p.KhachHangs)
                    .HasForeignKey(d => d.IdAvatar)
                    .HasConstraintName("FK_KhachHang_Avatar");

                entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.KhachHang)
                    .HasForeignKey<KhachHang>(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KhachHang_Users");


            });

            modelBuilder.Entity<LoaiTruyen>(entity =>
            {
                entity.HasKey(e => e.IdLoai);

                entity.ToTable("LoaiTruyen");

                entity.Property(e => e.IdLoai)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_loai");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.TenLoai)
                    .HasMaxLength(30)
                    .HasColumnName("ten_loai");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.IdUser).HasName("PK_NhanVien");

                // Định nghĩa thuộc tính IdUser với các ràng buộc cụ thể
                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .IsRequired();  // Không cho phép null

                // Định nghĩa thuộc tính StaffRole, cho phép null
                entity.Property(e => e.StaffRole).HasColumnName("StaffRole");

                // Cấu hình quan hệ một-một giữa NhanVien và User
                entity.HasOne(d => d.IdUserNavigation)  // NhanVien có một User
                    .WithOne(p => p.NhanVien)  // User có một NhanVien
                    .HasForeignKey<NhanVien>(d => d.IdUser)  // Khóa ngoại là IdUser
                    .OnDelete(DeleteBehavior.ClientSetNull)  // Thiết lập hành vi khi xóa là ClientSetNull
                    .HasConstraintName("FK_NhanVien_Users");  // Tên ràng buộc khóa ngoại

                // Cấu hình cho navigation property StaffPermissionsDetails
                entity.HasMany(e => e.StaffPermissionsDetails)
                    .WithOne(d => d.IdUserNavigation)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_StaffPermissionsDetails_NhanVien");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.IdPayment).HasName("PK__Payment__613289C0E7CD2C77");

                entity.ToTable("Payment");

                entity.Property(e => e.IdPayment)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.IdPack)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.PayAmount).HasColumnType("decimal(7, 2)");

                entity.HasOne(d => d.IdPackNavigation).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.IdPack)
                    .HasConstraintName("FK_Payment_Service_Pack_Config");

                entity.HasOne(d => d.IdPaymentNavigation).WithOne(p => p.Payment)
                    .HasForeignKey<Payment>(d => d.IdPayment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_KhachHang");
            });

            modelBuilder.Entity<PermissionsList>(entity =>
            {
                entity.HasKey(e => e.IdPermissions).HasName("PK__Permissi__0671E6D9E28B434E");

                entity.ToTable("PermissionsList");

                entity.Property(e => e.Description).HasMaxLength(50);
                entity.Property(e => e.ParentPermissions).HasColumnName("Parent_Permissions");
                entity.Property(e => e.PermissionsName).HasMaxLength(20);
            });

            modelBuilder.Entity<ServicePackConfig>(entity =>
            {
                entity.HasKey(e => e.IdPack).HasName("PK__Service___FC84C5ABF37A7BFC");

                entity.ToTable("Service_Pack_Config");

                entity.Property(e => e.IdPack)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Amount).HasColumnType("decimal(7, 2)");
                entity.Property(e => e.Description).HasMaxLength(60);
                entity.Property(e => e.Discount).HasColumnType("decimal(7, 2)");
                entity.Property(e => e.PackName).HasMaxLength(20);
                entity.Property(e => e.ParentPack)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("Parent_Pack");
            });

            modelBuilder.Entity<StaffActiveLog>(entity =>
            {
                entity.HasKey(e => e.IdLog).HasName("PK_Staff_Active_Logs_1");

                entity.ToTable("Staff_Active_Logs");

                entity.Property(e => e.IdLog)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.ChangeDescription).HasMaxLength(50);
                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.TimeChanged).HasColumnType("datetime");

                entity.HasOne(d => d.StaffPermissionsDetail).WithMany(p => p.StaffActiveLogs)
                    .HasForeignKey(d => new { d.IdUser, d.IdPermissions })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Active_Logs_Staff_Permissions_Detail");
            });

            modelBuilder.Entity<StaffPermissionsDetail>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdPermissions }).HasName("PK__Staff_Pe__37AE38558BE70804");

                entity.ToTable("Staff_Permissions_Detail");

                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdPermissionsNavigation).WithMany(p => p.StaffPermissionsDetails)
                    .HasForeignKey(d => d.IdPermissions)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Permissions_Detail_PermissionsList");

                entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.StaffPermissionsDetails)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Permissions_Detail_NhanVien");
            });

            modelBuilder.Entity<TacGium>(entity =>
            {
                entity.HasKey(e => e.IdTg);

                entity.Property(e => e.IdTg)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_tg");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.TenTg)
                    .HasMaxLength(30)
                    .HasColumnName("ten_tg");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser).HasName("PK__Users__B7C92638206D968B");

                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.FullName).HasMaxLength(30);
                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(u => u.NhanVien) // Mỗi User có thể có không hoặc một NhanVien
                    .WithOne(nv => nv.IdUserNavigation) // Mỗi NhanVien được liên kết chặt chẽ với một User
                    .HasForeignKey<NhanVien>(nv => nv.IdUser) // NhanVien sử dụng IdUser như là khóa ngoại
                    .OnDelete(DeleteBehavior.ClientSetNull) // Nếu User bị xóa, IdUser trong NhanVien sẽ được thiết lập là NULL
                    .HasConstraintName("FK_User_NhanVien");

                entity.HasOne(u => u.KhachHang) // Mỗi User có thể có không hoặc một KhachHang
                    .WithOne(kh => kh.IdUserNavigation) // Mỗi KhachHang được liên kết chặt chẽ với một User
                    .HasForeignKey<KhachHang>(kh => kh.IdUser) // KhachHang sử dụng IdUser như là khóa ngoại
                    .OnDelete(DeleteBehavior.ClientSetNull) // Nếu User bị xóa, IdUser trong KhachHang sẽ được thiết lập là NULL
                    .HasConstraintName("FK_User_KhachHang");
            });

            modelBuilder.Entity<WebMediaConfig>(entity =>
            {
                entity.HasKey(e => e.IdConfig).HasName("PK__WebMedia__79F21764D24494B8");

                entity.ToTable("WebMedia_Config");

                entity.Property(e => e.IdConfig).ValueGeneratedNever();
                entity.Property(e => e.ConfigTitle).HasMaxLength(30);
            });



            //OnModelCreatingPartial(modelBuilder);

        }
        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}


