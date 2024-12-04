using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAPI.Models;

[Table("sach_kieu_sach")] //kieu cua sach (manga, light novel, ...)
public class SachKieuSach
{
    [Column("ma_sach")] //ma sach
    public int MaSach { get; set; } //ma sach
    [Column("ma_kieu_sach")] //ma kieu sach
    public int MaKieuSach { get; set; } //ma kieu sach

    // bien nay se tu dong lay du lieu tu bang Sach
    public Sach Sach { get; set; } //sach
    // bien nay se tu dong lay du lieu tu bang KieuSach
    public KieuSach KieuSach { get; set; } //kieu sach
}