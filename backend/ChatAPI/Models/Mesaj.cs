// Model (Veritabanı Tablosu) Oluşturma

using System;
using System.ComponentModel.DataAnnotations;

namespace ChatAPI.Models;
public class Mesaj
{
    [Key]
    public int Id { get; set;}
    public string? KullaniciAdi { get; set;} // Hangi kullanıcı yazdı  ? Bu alan boş olabilir
    public string? Metin { get; set;} // Mesajın içeriği
    public DateTime GonderilmeZamani { get; set;} = DateTime.Now; // Ne zaman gönderildi?
    public string? Duygu { get; set; } // AI'nin analiz sonucu (pozitif/negatif/nötr)
    public double? DuyguSkoru { get; set; } // AI'nin güven skoru (0-1 arası)
}