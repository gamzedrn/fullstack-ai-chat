// DbContext (Veritabanı Köprüsü) Oluşturma, veritabanının anlayacağı dile çeviriyor.

using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Bu, veritabanında 'Mesajlar' adında bir tablo olacağını belirtir.
    public DbSet<Mesaj> Mesajlar { get; set; }
}


//  Entity Framework Core kütüphanesi sayesinde SQL sorgusu yazmaya gerek kalmıyor, C# kodunu otomatik olarak veritabanı işlemine çeviriyor
// AppDbContext adında yeni bir sınıf tanımlanıyor
// : DbContext kısmı ise bu sınıfın DbContext'in tüm özellikleirni ve fonksiyonlarını aldığı anlamına geliyor
// : base(options) → Gelen ayarları DbContext'e iletmek için yazılıyor.
// DbSet → Veritabanındaki bir tabloyu temsil ediyor.
// Mesajlar → Tablonun adı. Veritabanında bu isimde bir tablo oluşturuluyor.
// <Mesaj> → Bu tablonun her satırı Mesaj.cs'de tanımladığımız yapıya sahip olacak