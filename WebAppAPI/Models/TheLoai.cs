using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAPI.Models;

[Table("the_loai")]
public class TheLoai
{
    [Key]
    [Column("ma_the_loai")]
    public int MaTheLoai { get; set; }
    [Column("ten_the_loai")]
    public string TenTheLoai { get; set; }
    [Column("ma_the_loai_cha")]
    public int? MaTheLoaiCha { get; set; }

    public TheLoai TheLoaiCha { get; set; }
    public virtual ICollection<TheLoai> TheLoaiCon { get; set; }
    public virtual ICollection<SachTheLoai> SachTheLoais { get; set; }
}