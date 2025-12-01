using LastLink.Anticipation.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace LastLink.Anticipation.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AnticipationRequest> Anticipations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnticipationRequest>().HasKey(x => x.Id);
            modelBuilder.Entity<AnticipationRequest>().Property(x => x.Status).HasConversion<string>();
        }
    }
}