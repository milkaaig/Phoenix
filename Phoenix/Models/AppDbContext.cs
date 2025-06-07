
using Microsoft.EntityFrameworkCore;
using Phoenix.Data;

namespace Phoenix.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet for Posts table
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Post entity
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Description)
                      .HasMaxLength(1000);

                entity.Property(e => e.Category)
                      .HasMaxLength(100);

                entity.Property(e => e.Author)
                      .HasMaxLength(100);

                entity.Property(e => e.Content)
                      .IsRequired();

                // Configure date as required
                entity.Property(e => e.date)
                      .IsRequired();
            });
        }
    }
}
