using Microsoft.EntityFrameworkCore;
using NewsBlog.Models.Models;
using System.Reflection.Metadata;

namespace NewsBlog.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(b => b.BlogId);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(250);
            entity.Property(b => b.Content).IsRequired();
            entity.Property(b => b.Views).HasDefaultValue(0);
            entity.HasMany(b => b.Comments).WithOne(c => c.Blog).HasForeignKey(c => c.BlogId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(b => b.Likes).WithOne(l => l.Blog).HasForeignKey(l => l.BlogId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.CommentId);
            entity.Property(c => c.Author).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Content).IsRequired();
            entity.HasOne(c => c.Blog).WithMany(b => b.Comments).HasForeignKey(c => c.BlogId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(l => l.LikeId);
            entity.Property(l => l.UserId).IsRequired().HasMaxLength(50);
            entity.HasOne(l => l.Blog).WithMany(b => b.Likes).HasForeignKey(l => l.BlogId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);
        });
    }
}