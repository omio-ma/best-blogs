using BestBlogs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BestBlogs.Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CommenterName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.CommenterEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Content)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(c => c.IsSpam)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        // Indexes for performance
        builder.HasIndex(c => new { c.PostId, c.CreatedAt })
            .IsDescending(false, true); // PostId ascending, CreatedAt descending

        // Partial index for fetching non-spam comments
        builder.HasIndex(c => c.PostId)
            .HasFilter("is_spam = false");
    }
}
