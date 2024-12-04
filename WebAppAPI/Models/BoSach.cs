using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

 namespace WebAppAPI.Models;

 [Table("bo_sach")]
 public class BoSach
 {
     [Key]
     [Column("ma_bo_sach")]
     public int MaBoSach { get; set; }
     [Column("ten_bo_sach")]
     public string TenBoSach { get; set; }

     [NotMapped]
     public virtual ICollection<SachBoSach> SachBoSachs { get; set; }
 }