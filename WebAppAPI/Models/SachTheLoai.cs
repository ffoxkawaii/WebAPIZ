using System.ComponentModel.DataAnnotations.Schema;
namespace WebAppAPI.Models;

[Table("sach_the_loai")]
public class SachTheLoai
{
    [Column("ma_sach")]
    public int MaSach { get; set; }
    [NotMapped]
    public Sach sach { get; set; }

    [Column("ma_the_loai")]
    public int MaTheLoai { get; set; }
    [ForeignKey("MaTheLoai")]
    public virtual TheLoai TagTheLoai { get; set; } // Adjusted for consistency
}