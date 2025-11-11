using Microsoft.EntityFrameworkCore;  // EF Core, C# ile veritabaný iþlemleri yapmamýzý saðlar 

namespace ChatApi.Models
{
    // DbCContext, EF Core'da veritabaný ile uygulama arasýndaki ana baðlantýyý saðlar
    public class ChatDbContext : DbContext
    {
        // Constructor: DbContextOptions ile konfigürasyonlarý alýr
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        //Messages tablosuna karþýlýk gelen DbSet
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Message entity'sini "Messages" adýnda tabloya eþle
            modelBuilder.Entity<Message>().ToTable("Messages");
        }
    }
}