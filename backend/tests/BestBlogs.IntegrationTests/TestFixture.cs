using System;
using System.Net.Http;
using System.Threading.Tasks;
using BestBlogs.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Xunit;

namespace BestBlogs.IntegrationTests;

public class TestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private WebApplicationFactory<Program>? _factory;

    public HttpClient Client { get; private set; } = null!;
    public IServiceProvider Services { get; private set; } = null!;

    public TestFixture()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .Build();
    }

    public async Task InitializeAsync()
    {
        // Start the PostgreSQL container
        await _dbContainer.StartAsync();

        // Create the web application factory
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    services.RemoveAll(typeof(DbContextOptions<BestBlogsDbContext>));
                    services.RemoveAll(typeof(BestBlogsDbContext));

                    // Add DbContext with test container connection string
                    var connectionString = _dbContainer.GetConnectionString();
                    services.AddDbContext<BestBlogsDbContext>(options =>
                    {
                        options.UseNpgsql(connectionString);
                    });

                    // Build service provider and run migrations
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<BestBlogsDbContext>();

                    // Ensure database is created and migrated
                    db.Database.Migrate();
                });
            });

        Client = _factory.CreateClient();
        Services = _factory.Services;
    }

    public async Task DisposeAsync()
    {
        Client?.Dispose();
        _factory?.Dispose();
        await _dbContainer.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BestBlogsDbContext>();

        // Clear all data but keep the schema
        dbContext.BlogPosts.RemoveRange(dbContext.BlogPosts);
        dbContext.Comments.RemoveRange(dbContext.Comments);
        dbContext.Categories.RemoveRange(dbContext.Categories.Where(c => c.Name != "General"));
        dbContext.AdminUsers.RemoveRange(dbContext.AdminUsers);

        await dbContext.SaveChangesAsync();
    }
}
