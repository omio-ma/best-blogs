using System;
using System.Collections.Generic;

namespace BestBlogs.Domain.Entities;

/// <summary>
/// Represents a blog post with content, metadata, and categorization
/// </summary>
public class BlogPost
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public Category Category { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
