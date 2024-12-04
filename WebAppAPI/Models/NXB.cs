using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAPI.Models
{
    [Table("nha_xuat_ban")]
    public class NXB
    {
        [Key]
        [Column("ma_nha_xuat_ban")]
        public int MaNhaXuatBan { get; set; }
        [Column("ten_nha_xuat_ban")]
        public string TenNhaXuatBan { get; set; }
        [NotMapped]
        public virtual ICollection<Sach> Sach { get; set; }
    }
}