using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebAppAPI.Models;

[Table("don_hang_chi_tiet")]
public class DonHangChiTiet
{
    [Key]
    [ForeignKey("ma_don_hang")]
    [Column("ma_don_hang")]
    public int MaDonHang { get; set; }
    public DonHang DonHang { get; set; }
    [Key]
    [ForeignKey("ma_sach")]
    [Column("ma_sach")]
    public int MaSach { get; set; }
    [NotMapped]
    public Sach Sach { get; set; }

    [Column("so_luong")]
    public int SoLuong { get; set; }
    [Column("gia_tien")]
    public decimal GiaTien { get; set; }
}