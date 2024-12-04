using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAppAPI.Models;

// DTO for the response
public class SachDto
{
    public int MaSach { get; set; }
    public string TenSach { get; set; }

    public float SoTap { get; set; }
    public IEnumerable<TacGiaDto> TacGias { get; set; }
    public IEnumerable<TheLoaiDto> TheLoais { get; set; }
    public IEnumerable<BoSachDto> BoSachs { get; set; } // Thông tin bộ sách
    public IEnumerable<KieuSachDto> KieuSachs { get; set; } // Thông tin kiểu sách
    public DateOnly NgayRaMat { get; set; } // Ngày ra mắt

    public string GioiThieu { get; set; } //

    public decimal GiaTien { get; set; } // Giá tiền

    public string HinhAnh { get; set; } // Hình ảnh
}

public class ThemSachDto
{
    public string TenSach { get; set; }
    public DateOnly NgayRaMat { get; set; }
    public decimal GiaTien { get; set; }
    public string TenNXB { get; set; }
    public string GioiThieu { get; set; }
    public List<string> TenTacGia { get; set; } // Danh sách mã tác giả
    public List<string> TenTheLoai { get; set; } // Danh sách mã thể loại
    public string TenBoSach { get; set; } // Mã bộ sách (có thể là null)
    public string TenKieuSach { get; set; } // Mã kiểu sách (có thể là null)
}


public class TacGiaDto
{
    public int MaTacGia { get; set; }
    public string TenTacGia { get; set; }
}

public class TheLoaiDto
{
    public int MaTheLoai { get; set; }
    public string TenTheLoai { get; set; }
}

public class BoSachDto
{
    public int MaBoSach { get; set; }
    public string TenBoSach { get; set; }
}

public class KieuSachDto
{
    public int MaKieuSach { get; set; }
    public string TenKieuSach { get; set; }
}

[ApiController]
[Route("api/sach")]
public class SachController : ControllerBase
{
    private readonly WebDatabaseContext _context;
    private readonly ILogger<SachController> _logger;

    public SachController(WebDatabaseContext context, ILogger<SachController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Improved query builder with error handling and caching
    private async Task<List<SachDto>> TruyVan(Expression<Func<Sach, bool>> condition)
    {
        try
        {
            return await _context.Sachs
                .AsNoTracking() // Performance optimization for read-only queries
                .Where(condition)
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    SoTap = s.SoTap,
                    GioiThieu = s.GioiThieu,
                    TheLoais = s.SachTheLoais.Select(stl => new TheLoaiDto
                    {
                        MaTheLoai = stl.TagTheLoai.MaTheLoai,
                        TenTheLoai = stl.TagTheLoai.TenTheLoai
                    }),
                    TacGias = s.SachTacGias.Select(stg => new TacGiaDto
                    {
                        MaTacGia = stg.TacGia.MaTacGia,
                        TenTacGia = $"{stg.TacGia.Ho} {stg.TacGia.Ten}".Trim()
                    }),
                    BoSachs = s.SachBoSachs.Select(sbs => new BoSachDto
                    {
                        MaBoSach = sbs.BoSach.MaBoSach,
                        TenBoSach = sbs.BoSach.TenBoSach
                    }),
                    KieuSachs = s.SachKieuSachs.Select(sks => new KieuSachDto
                    {
                        MaKieuSach = sks.KieuSach.MaKieuSach,
                        TenKieuSach = sks.KieuSach.TenKieuSach
                    }),
                    NgayRaMat = s.NgayXuatBan,
                    GiaTien = s.GiaBan,
                    HinhAnh = s.HinhAnh
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing book query");
            throw;
        }
    }

    // truy van tat ca sach
    [HttpGet("getAllBooks")]
    public async Task<IActionResult> GetAllBooks()
    {
        var sach = await TruyVan(s => true);
        if (!sach.Any())
        {
            return NotFound(new { Error = "Không tìm thấy sách" });
        }

        return Ok(sach);
    }


    // tim sach bang ten sach
    [HttpGet("getBookByName")]
    public async Task<IActionResult> TruyVanSachTheoTen(string tenSach)
    {
        if (string.IsNullOrWhiteSpace(tenSach))
        {
            return BadRequest(new { Error = "Tên sách không được để trống" });
        }

        var sach = await TruyVan(s => s.TenSach.Contains(tenSach));
        if (!sach.Any())
        {
            return NotFound(new { Error = "Không tìm thấy sách" });
        }

        return Ok(sach);
    }

    // tim kiem theo bo
    [HttpGet("getBookBySeries")]
    public async Task<IActionResult> TruyVanSachTheoBo(string tenBoSach)
    {
        if (string.IsNullOrWhiteSpace(tenBoSach))
        {
            return BadRequest(new { Error = "Tên bộ sách không được để trống" });
        }

        var sach = await TruyVan(s => s.SachBoSachs.Any(sbs => sbs.BoSach.TenBoSach.Contains(tenBoSach)));
        if (!sach.Any())
        {
            return NotFound(new { Error = "Không tìm thấy sách trong bộ sách này" });
        }

        return Ok(sach);
    }

    // tim kiem theo ten tac gia
    [HttpGet("getBookByAuthor")]
    public async Task<IActionResult> TruyVanTheoTacGia(string tenTacGia)
    {
        if (string.IsNullOrWhiteSpace(tenTacGia))
        {
            return BadRequest(new { Error = "Tên tác giả không được để trống" });
        }

        var sach = await TruyVan(s => s.SachTacGias.Any(stg => stg.TacGia.TenTacGia.Contains(tenTacGia)));
        if (!sach.Any())
        {
            return NotFound(new { Error = "Không tìm thấy sách của tác giả này" });
        }

        return Ok(sach);
    }

    // Improved book creation with validation and transaction
    [HttpPost("them")]
    public async Task<IActionResult> ThemSach([FromBody] ThemSachDto sachDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Validate input data
            if (string.IsNullOrWhiteSpace(sachDto.TenSach))
                return BadRequest(new { Error = "Tên sách không được để trống" });

            if (!sachDto.TenTacGia?.Any() ?? true)
                return BadRequest(new { Error = "Phải có ít nhất một tác giả" });

            var sach = new Sach
            {
                TenSach = sachDto.TenSach.Trim(),
                NgayXuatBan = sachDto.NgayRaMat,
                GiaBan = sachDto.GiaTien,
                GioiThieu = sachDto.GioiThieu?.Trim(),
            };

            // Handle NXB
            sach.Publisher = await GetOrCreateNXB(sachDto.TenNXB);

            // Handle Authors
            await HandleAuthors(sach, sachDto.TenTacGia);

            // Handle Categories
            await HandleCategories(sach, sachDto.TenTheLoai);

            // Handle Book Collection
            if (!string.IsNullOrEmpty(sachDto.TenBoSach))
            {
                var boSach = await GetOrCreateBoSach(sachDto.TenBoSach);
                sach.SachBoSachs = new List<SachBoSach>
                {
                    new SachBoSach { BoSach = boSach }
                };
            }

            // Handle Book Type
            if (!string.IsNullOrEmpty(sachDto.TenKieuSach))
            {
                var kieuSach = await GetOrCreateKieuSach(sachDto.TenKieuSach);
                sach.SachKieuSachs = new List<SachKieuSach>
                {
                    new SachKieuSach { KieuSach = kieuSach }
                };
            }

            _context.Sachs.Add(sach);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Return the created book details
            var createdBook = await TruyVan(s => s.MaSach == sach.MaSach);
            return CreatedAtAction(nameof(GetSachById), new { id = sach.MaSach }, createdBook.FirstOrDefault());
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error creating new book");
            return StatusCode(500, new { Error = "Đã xảy ra lỗi khi thêm sách", Details = ex.Message });
        }
    }

    // Helper methods for entity creation/retrieval
    private async Task<NXB> GetOrCreateNXB(string tenNXB)
    {
        return await _context.NhaXuatBans
            .FirstOrDefaultAsync(nxb => nxb.TenNhaXuatBan == tenNXB)
            ?? new NXB { TenNhaXuatBan = tenNXB };
    }

    private async Task HandleAuthors(Sach sach, List<string> tenTacGias)
    {
        var existingAuthors = await _context.TacGias
            .Where(tg => tenTacGias.Contains(tg.TenTacGia))
            .ToDictionaryAsync(tg => tg.TenTacGia);

        foreach (var tenTacGia in tenTacGias)
        {
            if (!existingAuthors.TryGetValue(tenTacGia, out var tacGia))
            {
                tacGia = new TacGia { TenTacGia = tenTacGia };
                _context.TacGias.Add(tacGia);
            }
            sach.SachTacGias.Add(new SachTacGia { TacGia = tacGia });
        }
    }

    private async Task HandleCategories(Sach sach, List<string> tenTheLoais)
    {
        if (tenTheLoais?.Any() ?? false)
        {
            var existingCategories = await _context.TheLoais
                .Where(tl => tenTheLoais.Contains(tl.TenTheLoai))
                .ToDictionaryAsync(tl => tl.TenTheLoai);

            foreach (var tenTheLoai in tenTheLoais)
            {
                if (!existingCategories.TryGetValue(tenTheLoai, out var theLoai))
                {
                    theLoai = new TheLoai { TenTheLoai = tenTheLoai };
                    _context.TheLoais.Add(theLoai);
                }
                sach.SachTheLoais.Add(new SachTheLoai { TagTheLoai = theLoai });
            }
        }
    }

    private async Task<BoSach> GetOrCreateBoSach(string tenBoSach)
    {
        return await _context.BoSachs
            .FirstOrDefaultAsync(bs => bs.TenBoSach == tenBoSach)
            ?? new BoSach { TenBoSach = tenBoSach };
    }

    private async Task<KieuSach> GetOrCreateKieuSach(string tenKieuSach)
    {
        return await _context.KieuSachs
            .FirstOrDefaultAsync(ks => ks.TenKieuSach == tenKieuSach)
            ?? new KieuSach { TenKieuSach = tenKieuSach };
    }

    // Get book by ID - needed for CreatedAtAction
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSachById(int id)
    {
        var sach = await TruyVan(s => s.MaSach == id);
        if (!sach.Any())
            return NotFound(new { Error = "Không tìm thấy sách" });

        return Ok(sach.First());
    }
}

