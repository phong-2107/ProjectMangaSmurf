using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Areas.Identity.Data;
using ProjectMangaSmurf.Models;
using System.Reflection.Emit;

namespace ProjectMangaSmurf.Data
{
    public class ProjectDBContext : IdentityDbContext<ApplicationUser>
    {
        public ProjectDBContext(DbContextOptions<ProjectDBContext> options)
            : base(options)
        {
        }

        public DbSet<BoTruyen> BoTruyens { get; set; }

        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<CtBoTruyen> CtBoTruyens { get; set; }

        public DbSet<CtChapter> CtChapters { get; set; }

        public DbSet<CtHoatDong> CtHoatDongs { get; set; }

        public DbSet<CtLoaiTruyen> CtLoaiTruyens { get; set; }

        public DbSet<Footer> Footers { get; set; }

        public DbSet<HopDong> HopDongs { get; set; }

        public DbSet<KhachHang> KhachHangs { get; set; }

        public DbSet<LoaiTruyen> LoaiTruyens { get; set; }

        public DbSet<NhanVien> NhanViens { get; set; }

        public DbSet<Premium> Premia { get; set; }

        public DbSet<TacGium> TacGia { get; set; }
        public DbSet<ContactMail> ContactMail { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BoTruyen>(entity =>
            {
                entity.HasKey(e => e.IdBo);

                entity.ToTable("BoTruyen");

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
                entity.Property(e => e.TenBo)
                    .HasMaxLength(30)
                    .HasColumnName("ten_bo");
                entity.Property(e => e.Mota)
                    .HasColumnName("mota")
                    .HasColumnType("nvarchar(max)");
                entity.Property(e => e.TkDanhgia).HasColumnName("tk_danhgia");

                entity.Property(e => e.TkTheodoi).HasColumnName("tk_theodoi");
                entity.Property(e => e.TrangThai).HasColumnName("trang_thai");
                entity.Property(e => e.TtPemium).HasColumnName("tt_pemium");
                entity.Property(e => e.TongLuotXem).HasColumnName("TongLuotXem");
                entity.HasOne(d => d.IdTgNavigation).WithMany(p => p.BoTruyens)
                    .HasForeignKey(d => d.IdTg)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoTruyen_TacGia");
            });

            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.HasKey(e => new { e.SttChap, e.IdBo });

                entity.ToTable("Chapter");
                entity.Property(e => e.SttChap).HasColumnName("stt_chap");
                entity.Property(e => e.IdBo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_bo");
                entity.Property(e => e.Active).HasColumnName("active");
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

            modelBuilder.Entity<CtBoTruyen>(entity =>
            {
                entity.HasKey(e => new { e.IdKh, e.IbBo });

                entity.ToTable("CT_BoTruyen");

                entity.Property(e => e.IdKh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_kh");
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

                entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.CtBoTruyens)
                    .HasForeignKey(d => d.IdKh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_BoTruyen_KhachHang");
            });

            modelBuilder.Entity<CtChapter>(entity =>
            {
                entity.HasKey(e => new { e.SoTrang, e.SttChap, e.IdBo });

                entity.ToTable("CT_Chapter");

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
                entity.HasKey(e => new { e.IdKh, e.SttChap, e.IdBo });

                entity.ToTable("CT_HoatDong");

                entity.Property(e => e.IdKh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_kh");
                entity.Property(e => e.SttChap).HasColumnName("stt_chap");
                entity.Property(e => e.IdBo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_bo");
                entity.Property(e => e.TtDoc).HasColumnName("tt_doc");

                entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.CtHoatDongs)
                    .HasForeignKey(d => d.IdKh)
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

            modelBuilder.Entity<Footer>(entity =>
            {
                entity
                    .HasKey(e => e.Giayphep);

                entity.ToTable("Footer");

                entity.Property(e => e.Dieukhoan)
                    .HasColumnType("text")
                    .HasColumnName("dieukhoan");
                entity.Property(e => e.Giayphep)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("giayphep");
                entity.Property(e => e.LinkFb)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("link_fb");
                entity.Property(e => e.LinkIns)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("link_ins");
                entity.Property(e => e.LinkX)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("link_x");
                entity.Property(e => e.Noidung)
                    .HasColumnType("nvarchar(max)")
                    .HasColumnName("noidung");
            });

            modelBuilder.Entity<HopDong>(entity =>
            {
                entity.HasKey(e => e.IdHd);

                entity.ToTable("HopDong");

                entity.Property(e => e.IdHd)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_hd");
                entity.Property(e => e.GtThanhtoan)
                    .HasColumnType("decimal(6, 0)")
                    .HasColumnName("gt_thanhtoan");
                entity.Property(e => e.IdKh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_kh");
                entity.Property(e => e.NoiDung)
                   .HasColumnName("noidung")
                   .HasColumnType("nvarchar(max)");
                entity.Property(e => e.MaGiaoDich).HasColumnName("MaGiaoDich");
                entity.Property(e => e.Ngaylap)
                    .HasColumnType("datetime")
                    .HasColumnName("ngaylap");

                entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.HopDongs)
                    .HasForeignKey(d => d.IdKh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HopDong_KhachHang");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.IdKh);

                entity.ToTable("KhachHang");

                entity.Property(e => e.IdKh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_kh");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .HasColumnName("email");
                entity.Property(e => e.LienketFb)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lienket_fb");
                entity.Property(e => e.LienketGg)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("lienket_gg");
                entity.Property(e => e.Matkhau)
                    .HasColumnType("nvarchar(max)")
                    .IsUnicode(false)
                    .HasColumnName("matkhau");
                entity.Property(e => e.Sdt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("sdt");
                entity.Property(e => e.Taikhoan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("taikhoan");
                entity.Property(e => e.TenKh)
                    .HasMaxLength(30)
                    .HasColumnName("ten_kh");
                entity.Property(e => e.TtPremium).HasColumnName("tt_premium");
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
                entity.HasKey(e => e.IdNv);

                entity.ToTable("NhanVien");

                entity.Property(e => e.IdNv)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id_nv");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.LoaiNv).HasColumnName("loai_nv");
                entity.Property(e => e.Matkhau)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("matkhau");
                entity.Property(e => e.Taikhoan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("taikhoan");
                entity.Property(e => e.Ten)
                    .HasMaxLength(30)
                    .HasColumnName("ten");
            });

            modelBuilder.Entity<Premium>(entity =>
            {
               entity.HasKey(e => e.GiaThanh);

                entity.ToTable("Premium");

                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.GiaThanh)
                    .HasColumnType("decimal(6, 0)")
                    .HasColumnName("gia_thanh");
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

            modelBuilder.Entity<ContactMail>(entity =>
            {
                entity.HasKey(e => e.Email);
            });

            //OnModelCreatingPartial(modelBuilder);
        }
        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}


