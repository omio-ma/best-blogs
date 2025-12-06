# Technology Research: Lifestyle Blog Platform

**Date**: 2025-12-06
**Feature**: 001-blog-platform
**Updated**: 2025-12-06 (switched to .NET 10 + Aspire stack)

## Research Summary

This document captures the research and decision-making process for the technology stack and architecture of the lifestyle blog platform using .NET 10 Web API with .NET Aspire orchestration and Domain-Driven Design.

---

## 1. .NET 10 Web API with .NET Aspire

### Decision: ASP.NET Core 10 with DDD Layering + .NET Aspire Orchestration

**Rationale**:
- .NET 10 is the latest release with modern C# 13 features
- .NET Aspire provides cloud-native orchestration and service discovery
- Aspire Service Defaults for telemetry, health checks, and resilience
- Built-in support for distributed applications
- Strong typing with C# prevents many runtime errors
- Built-in dependency injection and middleware pipeline
- Excellent tooling (Visual Studio, Rider, VS Code with C# DevKit)

**DDD Layer Architecture with Aspire**:
```
.NET Aspire AppHost (Orchestration)
    ↓
├── API Project (Web API)
│   ├── API Layer (Controllers, middleware)
│   │   ↓
│   ├── Application Layer (Use cases via CQRS)
│   │   ↓
│   ├── Infrastructure Layer (EF Core, external services)
│   │   ↓
│   └── Domain Layer (Core business logic)
│
├── Frontend Project (React SPA)
└── PostgreSQL Resource (Aspire-managed)
```

**Aspire Benefits**:
- Service orchestration and discovery
- Built-in telemetry (OpenTelemetry)
- Health checks and resilience patterns
- Easy local development with service dependencies
- Configuration management across services

**DDD Benefits**:
- Clear separation of concerns
- Business logic isolated from infrastructure
- Testable without database or external dependencies
- Easy to understand and navigate

**Alternatives Considered**:
- **Node.js/Express**: Less type-safe, performance not as good for CPU-intensive tasks
- **Java/Spring Boot**: More verbose, heavier runtime footprint
- **Go**: Simpler but less mature for complex business logic patterns

---

## 2. Manual CQRS Pattern (No MediatR)

### Decision: Hand-Rolled CQRS with Custom Interfaces + FluentValidation

**Rationale**:
- Simplicity - no external CQRS library needed (YAGNI principle)
- Explicit command/query separation without magic
- Full control over handler registration and lifetime
- Works seamlessly with dependency injection
- Easier to understand and debug (no reflection-based dispatch)

**Implementation Pattern**:
```csharp
// Base interfaces
public interface ICommand<TResult>
{
}

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken ct = default);
}

public interface IQuery<TResult>
{
}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken ct = default);
}

// Command example
public record CreatePostCommand(
    string Title,
    string Content,
    Guid CategoryId
) : ICommand<Guid>;

// Handler example
public class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, Guid>
{
    private readonly IPostRepository _postRepository;
    private readonly IValidator<CreatePostCommand> _validator;

    public CreatePostCommandHandler(
        IPostRepository postRepository,
        IValidator<CreatePostCommand> validator)
    {
        _postRepository = postRepository;
        _validator = validator;
    }

    public async Task<Guid> HandleAsync(CreatePostCommand command, CancellationToken ct = default)
    {
        // Validate
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Business logic
        var post = new BlogPost
        {
            Title = command.Title,
            Content = command.Content,
            CategoryId = command.CategoryId
        };

        await _postRepository.CreateAsync(post, ct);
        return post.Id;
    }
}

// Validator
public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Content).NotEmpty().MinimumLength(100);
    }
}

// Controller usage
[ApiController]
[Route("api/admin/posts")]
public class AdminPostsController : ControllerBase
{
    private readonly ICommandHandler<CreatePostCommand, Guid> _createPostHandler;

    public AdminPostsController(ICommandHandler<CreatePostCommand, Guid> createPostHandler)
    {
        _createPostHandler = createPostHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command, CancellationToken ct)
    {
        var postId = await _createPostHandler.HandleAsync(command, ct);
        return CreatedAtAction(nameof(GetPost), new { id = postId }, postId);
    }
}

// DI Registration (in Program.cs)
builder.Services.AddScoped<ICommandHandler<CreatePostCommand, Guid>, CreatePostCommandHandler>();
builder.Services.AddScoped<IValidator<CreatePostCommand>, CreatePostCommandValidator>();
```

**Benefits**:
- Single responsibility - one handler per use case
- Easy to test handlers in isolation
- No hidden behaviors or pipeline magic
- Clean controller code (thin layer)
- Explicit dependencies visible in constructor
- No reflection overhead

**Alternatives Considered**:
- **MediatR**: Adds complexity with pipeline behaviors and reflection, overkill for our needs
- **Direct service injection**: Controllers become fat, harder to test
- **Full CQRS with Event Sourcing**: Over-engineering for current requirements

---

## 3. Entity Framework Core 10 with PostgreSQL

### Decision: EF Core 10 Code-First with Fluent API Configuration + Aspire Integration

**Rationale**:
- Type-safe LINQ queries
- Automatic change tracking
- Code-First migrations for version control
- PostgreSQL provider well-maintained by Npgsql team
- Excellent performance with proper indexes
- Aspire provides automatic connection string configuration
- Built-in health checks for database connectivity

**Configuration Approach**:
```csharp
public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.ToTable("blog_posts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(p => p.PublishedAt)
            .IsDescending();

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Posts)
            .HasForeignKey(p => p.CategoryId);
    }
}
```

**PostgreSQL-Specific Features**:
- JSONB for flexible data (future enhancements)
- Full-text search with GIN indexes (future)
- Array types for tags (future)
- Excellent indexing capabilities

**Best Practices**:
- Use AsNoTracking() for read-only queries
- Implement repository pattern only where abstraction needed
- Use IQueryable for flexible querying
- Configure indexes in Fluent API

**Alternatives Considered**:
- **Dapper**: More boilerplate, less type-safety, faster for simple queries
- **NHibernate**: Less mainstream, aging ecosystem
- **SQL Server**: PostgreSQL is open-source, better for blog workloads

---

## 4. ASP.NET Core Identity with OAuth

### Decision: ASP.NET Core Identity + External OAuth Providers

**Rationale**:
- Built-in framework for authentication/authorization
- OAuth 2.0 support for Google and GitHub
- Cookie-based authentication works well with SPA
- Secure by default (HTTPS, anti-forgery, etc.)

**OAuth Configuration**:
```csharp
services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = configuration["OAuth:Google:ClientId"];
        options.ClientSecret = configuration["OAuth:Google:ClientSecret"];
        options.CallbackPath = "/api/admin/auth/google/callback";
    })
    .AddGitHub(options =>
    {
        options.ClientId = configuration["OAuth:GitHub:ClientId"];
        options.ClientSecret = configuration["OAuth:GitHub:ClientSecret"];
        options.CallbackPath = "/api/admin/auth/github/callback";
    });
```

**Security Best Practices**:
- Use HTTPS in production (enforced)
- Implement CORS policies (restrict to frontend origin)
- Use secure, HTTP-only cookies
- Short session lifetimes with sliding expiration
- CSRF protection via anti-forgery tokens

**Alternatives Considered**:
- **IdentityServer/Duende**: Overkill for single application
- **Auth0**: External dependency, cost implications
- **Custom JWT**: More complex, security risks if not done right

---

## 5. SpecFlow for BDD with Gherkin

### Decision: SpecFlow + xUnit + AwesomeAssertions

**Rationale**:
- Gherkin syntax for acceptance tests (business-readable)
- SpecFlow is the leading BDD framework for .NET
- Integrates seamlessly with xUnit
- Living documentation (feature files describe behavior)
- AwesomeAssertions provides readable, expressive assertion syntax

**Feature File Example**:
```gherkin
Feature: Browse Blog Posts
  As a visitor
  I want to browse blog posts
  So that I can read interesting lifestyle content

  Scenario: View homepage with recent posts
    Given the blog has 15 published posts
    When I visit the homepage
    Then I should see 10 blog posts
    And I should see a "Load More" button
    And the posts should be ordered by published date (newest first)

  Scenario: Load more posts
    Given the blog has 15 published posts
    And I am on the homepage
    When I click the "Load More" button
    Then I should see 5 additional posts
    And the "Load More" button should disappear
```

**Step Definition Pattern**:
```csharp
[Binding]
public class BlogSteps
{
    private readonly ScenarioContext _context;
    private readonly HttpClient _client;

    [Given(@"the blog has (\d+) published posts")]
    public async Task GivenTheBlogHasPublishedPosts(int count)
    {
        // Seed test data
    }

    [When(@"I visit the homepage")]
    public async Task WhenIVisitTheHomepage()
    {
        var response = await _client.GetAsync("/api/blog/posts");
        _context["response"] = response;
    }

    [Then(@"I should see (\d+) blog posts")]
    public async Task ThenIShouldSeeBlogPosts(int count)
    {
        var response = (HttpResponse)_context["response"];
        var posts = await response.Content.ReadAsAsync<PostListDto>();
        posts.Posts.Count.ShouldBe(count); // AwesomeAssertions
    }
}
```

**Benefits**:
- Tests are executable specifications
- Non-technical stakeholders can read/validate requirements
- Catch regressions before deployment
- Documents expected behavior

**Alternatives Considered**:
- **Plain xUnit**: Less readable for non-developers
- **Cucumber (via .NET port)**: SpecFlow is more mature for .NET

---

## 6. Testcontainers for Integration Testing

### Decision: Testcontainers.PostgreSQL + xUnit

**Rationale**:
- Real PostgreSQL database for integration tests
- No mocking of database layer (test real behavior)
- Isolated test environment (fresh DB per test run)
- Docker-based, runs anywhere

**Test Setup Example**:
```csharp
public class IntegrationTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    public HttpClient Client { get; private set; }

    public IntegrationTestFixture()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase("testdb")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var connString = _dbContainer.GetConnectionString();
                    // Replace DbContext with test database
                });
            });

        Client = factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
```

**Benefits**:
- True integration tests (not unit tests with mocks)
- Catch database-specific issues
- Test migrations work correctly
- Confidence in production behavior

**Alternatives Considered**:
- **In-Memory Database**: Doesn't catch PostgreSQL-specific issues
- **Shared test database**: Parallel test issues, state pollution

---

## 7. .NET Aspire for Orchestration

### Decision: .NET Aspire AppHost + Service Defaults

**Rationale**:
- Cloud-native orchestration for distributed applications
- Automatic service discovery and configuration
- Built-in telemetry with OpenTelemetry
- Health checks and resilience patterns
- Simplifies local development experience
- Production-ready deployment descriptors

**Project Structure**:
```
BestBlogs.AppHost/          # Aspire orchestrator project
BestBlogs.ServiceDefaults/  # Shared service configuration
BestBlogs.API/              # Web API (uses ServiceDefaults)
```

**AppHost Configuration**:
```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL resource
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .AddDatabase("bestblogsdb");

// Add API project with database connection
var apiService = builder.AddProject<Projects.BestBlogs_API>("api")
    .WithReference(postgres);

// Add frontend (if hosting React via Aspire)
builder.AddNpmApp("frontend", "../frontend")
    .WithReference(apiService)
    .WithHttpEndpoint(env: "PORT");

builder.Build().Run();
```

**ServiceDefaults Extension**:
```csharp
public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        // Add default OpenTelemetry configuration
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation();
            });

        // Add default health checks
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());

        // Add service discovery
        builder.Services.AddServiceDiscovery();

        return builder;
    }
}
```

**Benefits**:
- Single command to run entire application (`dotnet run --project BestBlogs.AppHost`)
- Automatic connection string injection
- Built-in observability (metrics, traces, logs)
- Easy transition from local development to cloud deployment
- Dashboard for monitoring services in development
- Resilience patterns (retries, circuit breakers) built-in

**Development Workflow**:
```bash
# Start all services with Aspire
cd BestBlogs.AppHost
dotnet run

# Aspire dashboard available at http://localhost:15000
# API automatically connects to PostgreSQL
# Frontend automatically connects to API
```

**Alternatives Considered**:
- **Docker Compose**: Less integrated with .NET, no built-in telemetry
- **Tye**: Deprecated in favor of .NET Aspire
- **Manual orchestration**: More boilerplate, no service discovery

---

## 8. GitHub Actions for CI/CD

### Decision: Multi-stage workflows with path filters

**Rationale**:
- Native GitHub integration
- Free for public repositories, affordable for private
- Excellent .NET and Docker support
- Path-based triggering (only build changed projects)

**Workflow Strategy**:

**Backend CI** (on changes to `backend/**`):
```yaml
name: Backend CI

on:
  push:
    paths:
      - 'backend/**'
      - '.github/workflows/backend-ci.yml'

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Restore dependencies
        run: dotnet restore
        working-directory: ./backend

      - name: Build
        run: dotnet build --no-restore
        working-directory: ./backend

      - name: Unit Tests
        run: dotnet test BestBlogs.UnitTests
        working-directory: ./backend/tests

      - name: Integration Tests
        run: dotnet test BestBlogs.IntegrationTests
        working-directory: ./backend/tests

      - name: Acceptance Tests (SpecFlow)
        run: dotnet test BestBlogs.AcceptanceTests
        working-directory: ./backend/tests
```

**Frontend CI** (on changes to `frontend/**`):
```yaml
name: Frontend CI

on:
  push:
    paths:
      - 'frontend/**'
      - '.github/workflows/frontend-ci.yml'

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'

      - name: Install dependencies
        run: npm ci
        working-directory: ./frontend

      - name: Lint
        run: npm run lint
        working-directory: ./frontend

      - name: Component Tests
        run: npm test
        working-directory: ./frontend

      - name: Build
        run: npm run build
        working-directory: ./frontend
```

**Deployment** (on merge to main):
- Build Docker images
- Push to container registry
- Deploy to hosting platform
- Run smoke tests

**Alternatives Considered**:
- **Azure DevOps**: More complex for simple projects
- **Jenkins**: Requires self-hosting
- **CircleCI/Travis**: GitHub Actions more integrated

---

## 9. React as Thin Client

### Decision: React + React Query + Axios

**Rationale**:
- React Query handles all server state (caching, refetching, etc.)
- No Redux or complex state management needed
- All business logic in backend API
- React components are purely presentational

**Architecture**:
```typescript
// API Client
const blogApi = {
  getPosts: (limit: number, offset: number) =>
    axios.get<PostListResponse>('/api/blog/posts', { params: { limit, offset } }),

  getPost: (id: string) =>
    axios.get<PostDto>(`/api/blog/posts/${id}`),
};

// React Component (thin)
export function PostList() {
  const { data, isLoading, fetchNextPage } = useInfiniteQuery({
    queryKey: ['posts'],
    queryFn: ({ pageParam = 0 }) => blogApi.getPosts(10, pageParam),
    getNextPageParam: (lastPage) => lastPage.hasMore ? lastPage.offset + 10 : undefined,
  });

  if (isLoading) return <LoadingSpinner />;

  return (
    <>
      {data.pages.map(page =>
        page.posts.map(post => <PostCard key={post.id} post={post} />)
      )}
      {data.hasMore && <LoadMoreButton onClick={fetchNextPage} />}
    </>
  );
}
```

**Benefits**:
- Simple React components (no business logic)
- Automatic caching and background refetching
- Optimistic updates handled by React Query
- All validation happens on backend

**SEO Strategy**:
- Backend returns HTML with Open Graph meta tags
- React hydrates the client-side
- Critical content server-rendered (via backend templates if needed)

---

## Summary of Technology Decisions

| Category | Decision | Rationale |
|----------|----------|-----------|
| Backend Framework | ASP.NET Core 10 Web API | Latest .NET, C# 13 features, performance, strong typing |
| Orchestration | .NET Aspire | Service orchestration, telemetry, health checks, service discovery |
| Architecture | DDD with Manual CQRS | Clear separation, testable, maintainable, no library overhead |
| Database | PostgreSQL 16 | Open-source, robust, excellent for read-heavy workloads |
| ORM | Entity Framework Core 10 | Type-safe, Code-First migrations, LINQ queries, Aspire integration |
| Authentication | ASP.NET Core Identity + OAuth | Built-in, secure, Google/GitHub integration |
| Validation | FluentValidation | Expressive, testable, integrates with custom CQRS |
| Frontend Framework | React 18 + TypeScript | Component-based, strong ecosystem, type-safe |
| Styling | SCSS + CSS Modules | Variables support, scoped styles, maintainability |
| Server State | React Query | Automatic caching, background refetching, simple API |
| HTTP Client | Axios | Widely adopted, interceptors, TypeScript support |
| Backend Testing | xUnit + AwesomeAssertions | Standard for .NET, readable assertions |
| BDD/Acceptance | SpecFlow (Gherkin) | Business-readable tests, living documentation |
| Integration Tests | Testcontainers | Real database, isolated, Docker-based |
| CI/CD | GitHub Actions | Native integration, path-based triggers, free tier |
| Spam Filtering | Keyword blocklist + rate limiting | Simple, effective for current scale, YAGNI-compliant |

---

## Risk Assessment

### Technical Risks

1. **EF Core N+1 Queries**: Lazy loading can cause performance issues
   - **Mitigation**: Use Include() for eager loading, AsNoTracking() for read-only queries

2. **OAuth Session Management**: Cookie-based auth requires proper CORS configuration
   - **Mitigation**: Strict CORS policies, secure cookie settings, HTTPS only

3. **Docker Build Times**: .NET + React builds can be slow
   - **Mitigation**: Multi-stage Dockerfiles, layer caching, GitHub Actions caching

4. **PostgreSQL Connection Pool**: Exhaustion under load
   - **Mitigation**: Proper connection pooling configuration, monitoring

5. **SpecFlow Test Maintenance**: Feature files can get out of sync
   - **Mitigation**: Run acceptance tests in CI, treat as first-class code

### Mitigation Strategies

- Comprehensive testing at all layers (unit, integration, acceptance)
- Database query logging in development
- Application Insights/Serilog for production monitoring
- Load testing before launch
- Staged rollout with traffic monitoring

---

## Next Phase

Proceed to Phase 1: Data Model & Contracts generation with EF Core entities
