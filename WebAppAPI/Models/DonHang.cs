using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebAppAPI.Models;

[Table("don_hang")]
public class DonHang
{
    [Key]
    [Column("ma_don_hang")]
    public int MaDonHang { get; set; }
    [Column("ten_nguoi_nhan")]
    public string TenNguoiNhan { get; set; }
    [Column("sdt_nguoi_nhan")]
    public string SdtNguoiNhan { get; set; }
    [Column("dia_chi_nguoi_nhan")]
    public string DiaChiNguoiNhan { get; set; }

    public ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
}