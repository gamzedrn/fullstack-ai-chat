// AI Servisi İçin Yardımcı Sınıf (Service) Oluşturma
// .NET'in AI servisine nasıl HTTP isteği atacağını gösterir

using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatAPI.Services;

public class AIService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    // readonly → Bir kez değer atandıktan sonra değiştirilemez. Constructor'da atanır, sonra sabit kalır.
    // _ (alt çizgi) → private değişkenlerin önüne _ koymak bir yazım kuralı. Zorunlu değil ama yaygın kullanım.

    public AIService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<(string Duygu, double Skor)> DuyguAnaliziYap(string metin)
    {
        // Hugging Face'teki Gradio uygulamızın API adresi
        // Gradio, otomatik olarak /api/predict adresinde bir API oluşturur.

        var apiUrl = _configuration["AIService:https://gamze0707-duygu-analizi-servisi.hf.space"] + "/api/predict";

        // Gradio API'sine gönderilecek veri formatı özel. 
        // Genelde {"data": ["mesajınız"]} şeklinde olur

        var requestBody = new
        {
            data = new string[] { metin }
        };

        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            // Gradio'dan gelen cevap formatı: {"data": ["Duygu: Pozitif (Güven: %95.40)"]}
            // Bu biraz ham bir veri. İçinden duygu ve skoru ayıklamamız lazım.

            var jsonResponse = JObject.Parse(responseBody);
            var dataArray = jsonResponse["data"] as JArray;

            if(dataArray != null && dataArray.Count > 0)
            {
                var resultString = dataArray[0].ToString();


             if (resultString.Contains("Pozitif"))
                {
                    // Skoru ayıklamak biraz uğraştırır, şimdilik sabit bir değer dönelim
                    // veya skoru ayrıştırmayı sonra hallederiz.
                    return ("pozitif", 0.95); 
                }
                else if (resultString.Contains("Negatif"))
                {
                    return("negatif", 0.95);
                }
                               else
                {
                    return ("nötr", 0.50);
                }
            }
            return("bilinmiyor", 0.0);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"AI servisi hatası: {ex.Message}");
            return("hata", 0.0);
        }
    }
}
