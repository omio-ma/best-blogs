using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestBlogs.Domain.Entities;

namespace BestBlogs.Domain.Interfaces;

/// <summary>
/// Repository interface for Blog Post operations
/// </summary>
public interface IPostRepository
{
    /// <summary>
    /// Retrieves a paginated list of blog posts with optional category filtering
    /// </summary>
    Task<(List<BlogPost> Posts, int TotalCount)> GetPostsAsync(
        int limit,
        int offset,
        Guid? categoryId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single blog post by ID
    /// </summary>
    Task<BlogPost?> GetPostByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new blog post
    /// </summary>
    Task<BlogPost> CreatePostAsync(BlogPost post, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing blog post
    /// </summary>
    Task UpdatePostAsync(BlogPost post, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a blog post
    /// </summary>
    Task DeletePostAsync(Guid id, CancellationToken cancellationToken = default);
}
