using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestBlogs.Domain.Entities;
using BestBlogs.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BestBlogs.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly BestBlogsDbContext _context;

    public CategoryRepository(BestBlogsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Category?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        category.Id = Guid.NewGuid();
        category.CreatedAt = DateTime.UtcNow;

        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return category;
    }

    public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _context.Categories.FindAsync(new object[] { id }, cancellationToken);

        if (category != null)
        {
            // Prevent deletion of "General" category
            if (category.IsGeneralCategory())
            {
                throw new InvalidOperationException("Cannot delete the General category");
            }

            // Reassign all posts to "General" category
            var postsToReassign = await _context.BlogPosts
                .Where(p => p.CategoryId == id)
                .ToListAsync(cancellationToken);

            foreach (var post in postsToReassign)
            {
                post.CategoryId = Category.GeneralCategoryId;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
