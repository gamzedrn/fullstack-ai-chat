using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatApi.Models;
using System.Text.Json;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ChatDbContext _context; // veritabaný baðlantýsý
        private readonly HttpClient _httpClient; // AI servisine istek göndermek

        public MessagesController(ChatDbContext context)
        {
            _context = context;    // Veritabaný context ini constructor dan alýr
            _httpClient = new HttpClient(); // HTTP istekleri atmak için HttpClient oluþturur
        }

        // GET: api/messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            try
            {
                // mesajlarý CreatAt tarihine göre sýrala
                var messages = await _context.Messages
                    .OrderBy(m => m.CreatedAt)
                    .ToListAsync();

                // 200 ok yanýtý + mesaj listesi
                return Ok(messages);
            }
            catch (Exception ex)
            {
                // Eðer hata olursa 500 (Internal Server Error) döner
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/messages  Yeni bir mesaj ekler
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            try
            {
                // Kullanýcý adý veya mesaj boþsa hata döner
                if (string.IsNullOrWhiteSpace(message.Username) || string.IsNullOrWhiteSpace(message.Text))
                {
                    return BadRequest("Username and text are required");
                }

                // AI servisine istek at duygu analizini al
                var sentimentResult = await AnalyzeSentiment(message.Text);
                message.Sentiment = sentimentResult.Sentiment;
                message.SentimentScore = sentimentResult.Score;
                message.CreatedAt = DateTime.UtcNow;

                // mesajý veritabanýna ekle
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                // 201 Created yanýtý + yeni oluþturulan mesaj döner
                return CreatedAtAction(nameof(GetMessages), new { id = message.Id }, message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<SentimentResult> AnalyzeSentiment(string text)
        {
            try
            {
                // KENDÝ HUGGING FACE URL'ÝNÝ BURAYA YAZ
                var huggingFaceUrl = "https://huggingface.co/spaces/gamze0707/emotion-analysis-chat-tur/run/predict";

                var requestData = new { data = new[] { text } };

                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(30);

                var response = await client.PostAsJsonAsync(huggingFaceUrl, requestData);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"AI Response: {content}");

                    // Gradio API response formatý
                    var jsonDoc = JsonDocument.Parse(content);
                    var dataElement = jsonDoc.RootElement.GetProperty("data");
                    var firstItem = dataElement.EnumerateArray().First();

                    var sentiment = firstItem.GetProperty("sentiment").GetString() ?? "neutral";
                    var score = firstItem.GetProperty("score").GetDouble();
                    var label = firstItem.GetProperty("label").GetString() ?? "Nötr";

                    return new SentimentResult
                    {
                        Sentiment = sentiment,
                        Score = score,
                        Label = label
                    };
                }
                else
                {
                    Console.WriteLine($"AI service error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI analysis error: {ex.Message}");
            }

            // Fallback: basit metin analizi
            return AnalyzeTextManually(text);
        }

        private SentimentResult AnalyzeTextManually(string text)
        {
            string lowerText = text.ToLower();

            // Türkçe pozitif kelimeler
            var positiveWords = new[] { "güzel", "harika", "mükemmel", "teþekkür", "iyi", "süper", "muhteþem", "sevindim", "mutlu" };
            // Türkçe negatif kelimeler  
            var negativeWords = new[] { "kötü", "berbat", "sinir", "kýzgýn", "üzgün", "korkunç", "nefret", "beðenmedim" };

            if (positiveWords.Any(word => lowerText.Contains(word)))
                return new SentimentResult { Sentiment = "positive", Score = 0.8, Label = "Pozitif" };
            else if (negativeWords.Any(word => lowerText.Contains(word)))
                return new SentimentResult { Sentiment = "negative", Score = 0.7, Label = "Negatif" };
            else
                return new SentimentResult { Sentiment = "neutral", Score = 0.5, Label = "Nötr" };
        }


        public class SentimentResult
        {
            public string Sentiment { get; set; } = "neutral";
            public double Score { get; set; }
            public string Label { get; set; } = "Nötr";
        }



    }



