// Controllers/MesajlarController.cs
// Kullanıcı mesaj gönderdiğinde veya mesajları okumak istediğinde bu dosya devreye giriyor.

using ChatAPI.Models;
using ChatAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Controllers;

// Bu dosyanın internet adresi
[Route("api/[controller]")]
[ApiController]
public class MesajlarController : ControllerBase
{
    // AppDbContext → Veritabanıyla konuşmak için

    private readonly AppDbContext _context;
    
    // AIService → Python servisine istek göndermek için
    private readonly AIService _aiService;

    public MesajlarController(AppDbContext context, AIService aiService)
    {
        _context = context;
        _aiService = aiService;
    }

    // GET: api/Mesajlar
    // Tüm mesajları getir (en eskiden en yeniye)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mesaj>>> GetMesajlar()
    {
        return await _context.Mesajlar.OrderBy(m => m.GonderilmeZamani).ToListAsync();
    }

    // POST: api/Mesajlar
    // Yeni bir mesaj gönder
    [HttpPost]
    public async Task<ActionResult<Mesaj>> PostMesaj(Mesaj mesaj)
    {
        // 1. AI servisinden duygu analizini al
        var (duygu, skor) = await _aiService.DuyguAnaliziYap(mesaj.Metin);
        
        // 2. Mesaj nesnesine duygu bilgilerini ekle
        mesaj.Duygu = duygu;
        mesaj.DuyguSkoru = skor;
        mesaj.GonderilmeZamani = DateTime.Now;

        // 3. Veritabanına ekle ve kaydet
        _context.Mesajlar.Add(mesaj);
        await _context.SaveChangesAsync();

        // 4. Kaydedilmiş mesajı geri döndür (artık ID'si ve duygu bilgileri var)
        return CreatedAtAction(nameof(GetMesajlar), new { id = mesaj.Id }, mesaj);
    }
}