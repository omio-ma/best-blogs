using System;

namespace BestBlogs.Domain.Entities;

/// <summary>
/// Represents a visitor comment on a blog post
/// </summary>
public class Comment
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public string CommenterName { get; set; } = string.Empty;
    public string CommenterEmail { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsSpam { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public BlogPost Post { get; set; } = null!;
}
