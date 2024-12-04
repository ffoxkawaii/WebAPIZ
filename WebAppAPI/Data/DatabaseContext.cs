using Microsoft.EntityFrameworkCore;
using WebAppAPI.Models;

public class WebDatabaseContext : DbContext
{
    // Các DbSet đại diện cho các bảng trong cơ sở dữ liệu
    public DbSet<Sach> Sachs { get; set; }
    public DbSet<TacGia> TacGias { get; set; }
    public DbSet<SachTacGia> SachTacGias { get; set; }
    public DbSet<TheLoai> TheLoais { get; set; }
    public DbSet<SachTheLoai> SachTheLoais { get; set; }
    public DbSet<NXB> NhaXuatBans { get; set; }
    public DbSet<DonHang> DonHangs { get; set; }
    public DbSet<DonHangChiTiet> DonHangChiTiets { get; set; }
    public DbSet<BoSach> BoSachs { get; set; }              // Thêm bảng BoSach
    public DbSet<SachBoSach> SachBoSachs { get; set; }       // Thêm bảng SachBoSach
    public DbSet<KieuSach> KieuSachs { get; set; }           // Thêm bảng KieuSach
    public DbSet<SachKieuSach> SachKieuSachs { get; set; }   // Thêm bảng SachKieuSach

    public WebDatabaseContext(DbContextOptions<WebDatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cấu hình cho bảng Sach
        modelBuilder.Entity<Sach>()
            .HasOne(s => s.Publisher)
            .WithMany(n => n.Sach)
            .HasForeignKey(s => s.MaNhaXuatBan)
            .OnDelete(DeleteBehavior.Cascade);

        // Cấu hình bảng trung gian giữa Sách và Tác giả (SachTacGia)
        modelBuilder.Entity<SachTacGia>()
            .HasKey(stg => new { stg.MaSach, stg.MaTacGia }); // Khóa chính tổng hợp

        modelBuilder.Entity<SachTacGia>()
            .HasOne<Sach>(stg => stg.Sach)
            .WithMany(s => s.SachTacGias)
            .HasForeignKey(stg => stg.MaSach);

        // Cấu hình quan hệ giữa SachTacGia và TacGia
        modelBuilder.Entity<SachTacGia>()
            .HasOne<TacGia>(stg => stg.TacGia)
            .WithMany(tg => tg.SachTacGias)
            .HasForeignKey(stg => stg.MaTacGia);

        // Cấu hình bảng trung gian giữa Sách và Thể loại (SachTheLoai)
        modelBuilder.Entity<SachTheLoai>()
            .HasKey(stl => new { stl.MaSach, stl.MaTheLoai });

        modelBuilder.Entity<SachTheLoai>()
            .HasOne<Sach>(stl => stl.sach)
            .WithMany(s => s.SachTheLoais)
            .HasForeignKey(stl => stl.MaSach);

        modelBuilder.Entity<SachTheLoai>()
            .HasOne<TheLoai>(stl => stl.TagTheLoai)
            .WithMany(tl => tl.SachTheLoais)
            .HasForeignKey(stl => stl.MaTheLoai);

        // Cấu hình cho NhaXuatBan và Sach
        modelBuilder.Entity<NXB>()
            .HasMany<Sach>(nxb => nxb.Sach)
            .WithOne(s => s.Publisher)
            .HasForeignKey(s => s.MaNhaXuatBan);

        // Cấu hình cho DonHang và DonHangChiTiet
        modelBuilder.Entity<DonHangChiTiet>()
            .HasKey(dhct => new { dhct.MaDonHang, dhct.MaSach });

        modelBuilder.Entity<DonHangChiTiet>()
            .HasOne<DonHang>(dhct => dhct.DonHang)
            .WithMany(dh => dh.DonHangChiTiets)
            .HasForeignKey(dhct => dhct.MaDonHang);

        modelBuilder.Entity<DonHangChiTiet>()
            .HasOne<Sach>(dhct => dhct.Sach)
            .WithMany(s => s.DonHangChiTiets)
            .HasForeignKey(dhct => dhct.MaSach);

        // Cấu hình cho BoSach và Sach (bảng trung gian SachBoSach)
        modelBuilder.Entity<SachBoSach>()
            .HasKey(sbs => new { sbs.MaSach, sbs.MaBoSach });

        modelBuilder.Entity<SachBoSach>()
            .HasOne(sbs => sbs.Sach)
            .WithMany(s => s.SachBoSachs)
            .HasForeignKey(sbs => sbs.MaSach);

        modelBuilder.Entity<SachBoSach>()
            .HasOne(sbs => sbs.BoSach)
            .WithMany(bs => bs.SachBoSachs)
            .HasForeignKey(sbs => sbs.MaBoSach);

        // Cấu hình cho KieuSach và Sach (bảng trung gian SachKieuSach)
        modelBuilder.Entity<SachKieuSach>()
            .HasKey(sks => new { sks.MaSach, sks.MaKieuSach });

        modelBuilder.Entity<SachKieuSach>()
            .HasOne<Sach>(sks => sks.Sach)
            .WithMany(s => s.SachKieuSachs)
            .HasForeignKey(sks => sks.MaSach);

        modelBuilder.Entity<SachKieuSach>()
            .HasOne<KieuSach>(sks => sks.KieuSach)
            .WithMany(ks => ks.SachKieuSachs)
            .HasForeignKey(sks => sks.MaKieuSach);
    }
}
