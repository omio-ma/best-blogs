using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BestBlogs.Domain.Entities;
using BestBlogs.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace BestBlogs.AcceptanceTests.Support;

public class TestContext
{
    public HttpClient Client { get; }
    private readonly IServiceScope _scope;
    private readonly BestBlogsDbContext _dbContext;

    public TestContext(HttpClient client, IServiceProvider serviceProvider)
    {
        Client = client;
        _scope = serviceProvider.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<BestBlogsDbContext>();
    }

    public async Task SeedBlogPostsAsync(int count)
    {
        var category = await EnsureGeneralCategoryExistsAsync();

        var posts = new List<BlogPost>();
        for (int i = 1; i <= count; i++)
        {
            posts.Add(new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = $"Test Post {i}",
                Content = new string('a', 150) + $" This is the content for test post {i}.",
                Excerpt = $"This is the excerpt for test post {i}.",
                AuthorName = "Test Author",
                CategoryId = category.Id,
                PublishedAt = DateTime.UtcNow.AddDays(-i), // Newer posts have higher index
                CreatedAt = DateTime.UtcNow.AddDays(-i),
                UpdatedAt = DateTime.UtcNow.AddDays(-i),
                IsDeleted = false
            });
        }

        await _dbContext.BlogPosts.AddRangeAsync(posts);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SeedBlogPostsInCategoryAsync(int count, string categoryName)
    {
        var category = await EnsureCategoryExistsAsync(categoryName);

        var posts = new List<BlogPost>();
        for (int i = 1; i <= count; i++)
        {
            posts.Add(new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = $"{categoryName} Post {i}",
                Content = new string('a', 150) + $" This is the content for {categoryName} post {i}.",
                Excerpt = $"This is the excerpt for {categoryName} post {i}.",
                AuthorName = "Test Author",
                CategoryId = category.Id,
                PublishedAt = DateTime.UtcNow.AddDays(-i),
                CreatedAt = DateTime.UtcNow.AddDays(-i),
                UpdatedAt = DateTime.UtcNow.AddDays(-i),
                IsDeleted = false
            });
        }

        await _dbContext.BlogPosts.AddRangeAsync(posts);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SeedBlogPostWithTitleAsync(string title)
    {
        var category = await EnsureGeneralCategoryExistsAsync();

        var post = new BlogPost
        {
            Id = Guid.NewGuid(),
            Title = title,
            Content = new string('a', 150) + $" This is the full content for the post titled '{title}'.",
            Excerpt = $"This is the excerpt for '{title}'.",
            AuthorName = "Test Author",
            CategoryId = category.Id,
            PublishedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _dbContext.BlogPosts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Guid> GetCategoryIdByNameAsync(string categoryName)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name == categoryName);

        if (category == null)
        {
            throw new InvalidOperationException($"Category '{categoryName}' not found");
        }

        return category.Id;
    }

    public async Task<Guid> GetPostIdByTitleAsync(string title)
    {
        var post = await _dbContext.BlogPosts
            .FirstOrDefaultAsync(p => p.Title == title);

        if (post == null)
        {
            throw new InvalidOperationException($"Post with title '{title}' not found");
        }

        return post.Id;
    }

    private async Task<Category> EnsureGeneralCategoryExistsAsync()
    {
        return await EnsureCategoryExistsAsync("General");
    }

    private async Task<Category> EnsureCategoryExistsAsync(string categoryName)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name == categoryName);

        if (category == null)
        {
            category = new Category
            {
                Id = categoryName == "General"
                    ? new Guid("00000000-0000-0000-0000-000000000001")
                    : Guid.NewGuid(),
                Name = categoryName,
                Description = $"{categoryName} category description",
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        return category;
    }

    public async Task CleanupAsync()
    {
        _dbContext.BlogPosts.RemoveRange(_dbContext.BlogPosts);
        _dbContext.Categories.RemoveRange(_dbContext.Categories.Where(c => c.Name != "General"));
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _scope?.Dispose();
    }
}
