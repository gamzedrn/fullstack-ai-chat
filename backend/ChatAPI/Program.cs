using ChatAPI.Models;
using ChatAPI.Services;
using Microsoft.EntityFrameworkCore;


// 1. HAZIRLIK    → builder ile başlayan satırlar
// 2. ÇALIŞMA     → app ile başlayan satırlar

var builder = WebApplication.CreateBuilder(args);

// Veritabanı context'ini ekle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// HttpClient'i ve AIService'i ekle
builder.Services.AddHttpClient<AIService>(); 

// Programa "dışarıdan gelen istekleri karşılayacak controller dosyaları var" diyorsun.
builder.Services.AddControllers();


// Swagger, API'ni otomatik olarak test edebileceğin bir web sayfası oluşturuyor
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTP ile gelen istekleri HTTPS'e yönlendir (güvenlik)
app.UseHttpsRedirection();

//  Yetkisiz erişimleri engelle
app.UseAuthorization();

// Dışarıdan gelen istekleri doğru controller dosyasına yönlendir
app.MapControllers();

// Basitlik için, uygulama ilk açıldığında veritabanını oluşturmasını sağla
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated(); // Eğer yoksa veritabanını oluşturur
}

app.Run();