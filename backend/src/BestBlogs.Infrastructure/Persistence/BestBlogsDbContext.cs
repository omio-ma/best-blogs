using BestBlogs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BestBlogs.Infrastructure.Persistence;

public class BestBlogsDbContext : DbContext
{
    public BestBlogsDbContext(DbContextOptions<BestBlogsDbContext> options)
        : base(options)
    {
    }

    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Category> Categories => Set<Category>();
    // AdminUser will be added in Phase 5
    // public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BestBlogsDbContext).Assembly);
    }
}
