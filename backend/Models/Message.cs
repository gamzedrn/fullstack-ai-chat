using System.ComponentModel.DataAnnotations;

namespace ChatApi.Models
{
    //Models klasörü altýnda Message sýnýfý tanýmlanýyor
    public class Message
    {
        //Mesajýn benzersiz ID'si
        public int Id { get; set; }

        //kullanýcý adý, boþ býrakýlamaz
        [Required]
        public string Username { get; set; } = string.Empty;

        //mesajýn içeriði boþ býrakýlamaz
        [Required]
        public string Text { get; set; } = string.Empty;


        //mesajýn duygu durumu, varsayýlan "neutral
        public string Sentiment { get; set; } = "neutral";

        //modelin verdiði duygu skorunu saklar (0-1 arasý)
        public double SentimentScore { get; set; }

        // Mesajýn oluþturulma zamaný, UTC olarak kaydedilir
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}