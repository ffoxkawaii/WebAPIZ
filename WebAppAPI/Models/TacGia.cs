using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAPI.Models;

[Table("tac_gia")] // ten bang trong database
public class TacGia
{
    [Key] // khoa chinh
    [Column("ma_tac_gia")] // ten cot trong database
    public int MaTacGia { get; set; }

    [Column("ho")] // ten cot trong database
    public string Ho { get; set; }

    [Column("ten")] // ten cot trong database
    public string Ten { get; set; }

    [NotMapped] // danh dau la khong phai la cot trong database
    public string TenTacGia
    {
        get => $"{Ho} {Ten}"; // kết hợp họ và tên để tạo tên tác giả
        set
        {
            // Tùy chọn: có thể tách giá trị để gán cho Ho và Ten
            var names = value?.Split(' ') ?? new string[0];
            if (names.Length > 0)
            {
                Ho = names[0]; // Gán Ho cho phần đầu tiên
            }
            if (names.Length > 1)
            {
                Ten = names[1]; // Gán Ten cho phần thứ hai
            }
        }
    }
    public virtual ICollection<SachTacGia> SachTacGias { get; set; } // danh sach cac sach cua tac gia
}