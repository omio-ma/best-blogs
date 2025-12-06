using BestBlogs.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BestBlogs.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IHostApplicationBuilder builder)
    {
        // Add PostgreSQL with Aspire integration
        builder.AddNpgsqlDbContext<BestBlogsDbContext>("bestblogsdb", configureDbContextOptions: options =>
        {
            options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
            options.EnableDetailedErrors(builder.Environment.IsDevelopment());
        });

        // Repositories will be added in Phase 3
        // services.AddScoped<IBlogPostRepository, BlogPostRepository>();
        // services.AddScoped<ICommentRepository, CommentRepository>();
        // services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}
