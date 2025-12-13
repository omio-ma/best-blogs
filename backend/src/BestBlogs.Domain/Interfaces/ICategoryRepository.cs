using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestBlogs.Domain.Entities;

namespace BestBlogs.Domain.Interfaces;

/// <summary>
/// Repository interface for Category operations
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Retrieves all categories
    /// </summary>
    Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a category by ID
    /// </summary>
    Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a category by name
    /// </summary>
    Task<Category?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new category
    /// </summary>
    Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing category
    /// </summary>
    Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a category (except "General" category)
    /// </summary>
    Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default);
}
