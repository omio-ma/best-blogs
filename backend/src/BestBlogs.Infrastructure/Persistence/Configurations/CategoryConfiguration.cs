using BestBlogs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BestBlogs.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        // Seed the "General" category
        builder.HasData(new Category
        {
            Id = Category.GeneralCategoryId,
            Name = "General",
            Description = "Uncategorized posts",
            CreatedAt = new System.DateTime(2025, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)
        });
    }
}
