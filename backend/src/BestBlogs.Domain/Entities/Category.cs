using System;
using System.Collections.Generic;

namespace BestBlogs.Domain.Entities;

/// <summary>
/// Represents a blog post category for organization
/// Invariant: The "General" category (ID: 00000000-0000-0000-0000-000000000001) cannot be deleted
/// </summary>
public class Category
{
    public static readonly Guid GeneralCategoryId = new("00000000-0000-0000-0000-000000000001");

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();

    /// <summary>
    /// Checks if this is the protected "General" category
    /// </summary>
    public bool IsGeneralCategory() => Id == GeneralCategoryId;
}
