using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAPI.Models
{
    [Table("sach_bo_sach")]
    public class SachBoSach
    {
        [Key]
        [Column("ma_sach")]
        public int MaSach { get; set; }

        [Key]
        [Column("ma_bo_sach")]
        public int MaBoSach { get; set; }

        [ForeignKey("MaSach")]
        public virtual Sach Sach { get; set; }

        [ForeignKey("MaBoSach")]
        public virtual BoSach BoSach { get; set; }
    }
}