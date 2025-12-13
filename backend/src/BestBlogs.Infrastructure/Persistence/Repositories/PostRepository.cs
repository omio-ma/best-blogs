using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestBlogs.Domain.Entities;
using BestBlogs.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BestBlogs.Infrastructure.Persistence.Repositories;

public class PostRepository : IPostRepository
{
    private readonly BestBlogsDbContext _context;

    public PostRepository(BestBlogsDbContext context)
    {
        _context = context;
    }

    public async Task<(List<BlogPost> Posts, int TotalCount)> GetPostsAsync(
        int limit,
        int offset,
        Guid? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.BlogPosts
            .AsNoTracking()
            .Where(p => !p.IsDeleted && p.PublishedAt <= DateTime.UtcNow);

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var posts = await query
            .Include(p => p.Category)
            .OrderByDescending(p => p.PublishedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return (posts, totalCount);
    }

    public async Task<BlogPost?> GetPostByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.BlogPosts
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);
    }

    public async Task<BlogPost> CreatePostAsync(BlogPost post, CancellationToken cancellationToken = default)
    {
        post.Id = Guid.NewGuid();
        post.CreatedAt = DateTime.UtcNow;
        post.UpdatedAt = DateTime.UtcNow;

        if (post.PublishedAt == default)
        {
            post.PublishedAt = DateTime.UtcNow;
        }

        await _context.BlogPosts.AddAsync(post, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return post;
    }

    public async Task UpdatePostAsync(BlogPost post, CancellationToken cancellationToken = default)
    {
        post.UpdatedAt = DateTime.UtcNow;
        _context.BlogPosts.Update(post);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePostAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var post = await _context.BlogPosts.FindAsync(new object[] { id }, cancellationToken);
        if (post != null)
        {
            post.IsDeleted = true;
            post.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
