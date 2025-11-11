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
        private readonly ChatDbContext _context;
        private readonly HttpClient _httpClient;

        public MessagesController(ChatDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }

        // GET: api/messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            try
            {
                var messages = await _context.Messages
                    .OrderBy(m => m.CreatedAt)
                    .ToListAsync();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/messages
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message.Username) || string.IsNullOrWhiteSpace(message.Text))
                {
                    return BadRequest("Username and text are required");
                }

                // AI servisine istek at (þimdilik mock data)
                var sentimentResult = await AnalyzeSentiment(message.Text);
                message.Sentiment = sentimentResult.Sentiment;
                message.SentimentScore = sentimentResult.Score;
                message.CreatedAt = DateTime.UtcNow;

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

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
                // ÞÝMDÝLÝK MOCK DATA - Hugging Face URL'sini sonra ekleyeceðiz
                // var huggingFaceUrl = "https://huggingface.co/spaces/gamze0707/emotion-analysis-chat-tur";

                // Mock analiz - pozitif/negatif/neutral basit kontrolü
                string lowerText = text.ToLower();
                if (lowerText.Contains("güzel") || lowerText.Contains("harika") || lowerText.Contains("teþekkür") || lowerText.Contains("iyi"))
                {
                    return new SentimentResult { Sentiment = "positive", Score = 0.8 };
                }
                else if (lowerText.Contains("kötü") || lowerText.Contains("berbat") || lowerText.Contains("sinir") || lowerText.Contains("kýzgýn"))
                {
                    return new SentimentResult { Sentiment = "negative", Score = 0.7 };
                }
                else
                {
                    return new SentimentResult { Sentiment = "neutral", Score = 0.5 };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI analysis error: {ex.Message}");
                return new SentimentResult { Sentiment = "neutral", Score = 0.5 };
            }
        }
    }

    public class SentimentResult
    {
        public string Sentiment { get; set; } = "neutral";
        public double Score { get; set; }
    }
}