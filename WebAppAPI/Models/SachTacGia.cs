using System.ComponentModel.DataAnnotations.Schema;
namespace WebAppAPI.Models;

[Table("sach_tac_gia")] //ten bang trong database
public class SachTacGia
{
    [Column("ma_sach")] //ten cot trong database
    public int MaSach { get; set; }
    [Column("ma_tac_gia")] //ten cot trong database
    public int MaTacGia { get; set; }

    public Sach Sach { get; set; } // bien nay se tu dong lay du lieu tu bang Sach
    public TacGia TacGia { get; set; } // bien nay se tu dong lay du lieu tu bang TacGia
}