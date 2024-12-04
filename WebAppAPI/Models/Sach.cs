using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAPI.Models
{
    [Table("sach")] // ten bang trong database
    public class Sach
    {
        [Key] // khoa chinh
        [Column("ma_sach")] // ten cot trong database
        public int MaSach { get; set; }

        [Column("tieu_de")] // ten cot trong database
        public string TenSach { get; set; }

        [Column("so_tap")] // ten cot trong database
        public float SoTap { get; set; }

        [Column("tong_so_trang")] // ten cot trong database
        public int TongSoTrang { get; set; }

        // [Column("ma_isbn")] // ten cot trong database
        // public string MaISBN { get; set; }

        [Column("ngay_xuat_ban")] // ten cot trong database
        public DateOnly NgayXuatBan { get; set; }

        [Column("ma_nha_xuat_ban")] // ten cot trong database
        public int MaNhaXuatBan { get; set; }

        [Column("gia_tien")] // ten cot trong database
        public decimal GiaBan { get; set; }

        [Column("gioi_thieu")] // ten cot trong database
        public string GioiThieu { get; set; }

        [NotMapped] // cot hinh anh, se duoc update sau vi bay gio chua co hinh anh tren db
        public string HinhAnh { get; set; }      // Rename the navigation property to avoid conflict
        public NXB Publisher { get; set; }

        // Quan hệ nhiều-nhiều với BoSach qua bảng trung gian SachBoSach
        public virtual ICollection<SachBoSach> SachBoSachs { get; set; }

        // Quan hệ nhiều-nhiều với KieuSach qua bảng trung gian SachKieuSach
        public virtual ICollection<SachKieuSach> SachKieuSachs { get; set; }

        public virtual ICollection<SachTacGia> SachTacGias { get; set; }
        public virtual ICollection<SachTheLoai> SachTheLoais { get; set; }
        public virtual ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
    }
}