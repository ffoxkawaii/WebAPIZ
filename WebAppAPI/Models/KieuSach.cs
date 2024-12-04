using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAPI.Models;

[Table("kieu_sach")]
public class KieuSach
{
   [Key]
   [Column("ma_kieu_sach")]
   public int MaKieuSach { get; set; }

   [Column("ten_kieu_sach")]
   public string TenKieuSach { get; set; }

   [NotMapped]
   public ICollection<SachKieuSach> SachKieuSachs { get; set; }
}