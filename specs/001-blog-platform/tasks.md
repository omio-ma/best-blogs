# Implementation Tasks: Lifestyle Blog Platform

**Branch**: `001-blog-platform` | **Date**: 2025-12-06 | **Spec**: [spec.md](./spec.md)

This file contains the complete task breakdown for implementing the lifestyle blog platform using .NET 10 Web API with .NET Aspire orchestration, Domain-Driven Design, manual CQRS pattern, React + TypeScript frontend, and SpecFlow BDD testing.

## Task Format

```
- [ ] [T001] [P1] [US1] Task description with file path
```

- **[T001]**: Unique task ID
- **[P1]**: Parallelization marker (tasks with same P number can run in parallel)
- **[US1]**: User Story reference (US1 = Browse Posts, US2 = Comments, US3 = Admin)
- **File paths**: Included for easy navigation

## Constitution Compliance

All tasks follow the three core principles:
- **Modular Monolith Architecture**: DDD layers with clear bounded contexts + Aspire orchestration
- **Test-Required Development**: SpecFlow + xUnit + AwesomeAssertions tests for all features
- **Simplicity & YAGNI**: Manual CQRS (no MediatR), simple pagination, keyword spam filter

---

## Phase 1: Solution Setup & .NET Aspire Infrastructure

### 1.1 .NET 10 Solution Structure

- [ ] [T001] [P1] Create .NET 10 solution file `BestBlogs.sln` in `backend/` directory
- [ ] [T002] [P1] Create `BestBlogs.AppHost` Aspire project in `backend/src/BestBlogs.AppHost/`
- [ ] [T003] [P1] Create `BestBlogs.ServiceDefaults` class library in `backend/src/BestBlogs.ServiceDefaults/`
- [ ] [T004] [P1] Create `BestBlogs.Domain` class library project in `backend/src/BestBlogs.Domain/`
- [ ] [T005] [P1] Create `BestBlogs.Application` class library project in `backend/src/BestBlogs.Application/`
- [ ] [T006] [P1] Create `BestBlogs.Infrastructure` class library project in `backend/src/BestBlogs.Infrastructure/`
- [ ] [T007] [P1] Create `BestBlogs.API` web API project in `backend/src/BestBlogs.API/`
- [ ] [T008] [P1] Create `BestBlogs.UnitTests` xUnit project in `backend/tests/BestBlogs.UnitTests/`
- [ ] [T009] [P1] Create `BestBlogs.IntegrationTests` xUnit project in `backend/tests/BestBlogs.IntegrationTests/`
- [ ] [T010] [P1] Create `BestBlogs.AcceptanceTests` SpecFlow project in `backend/tests/BestBlogs.AcceptanceTests/`
- [ ] [T011] [P2] Add project references: Domain ← Application ← Infrastructure ← API
- [ ] [T012] [P2] Add API reference to AppHost for orchestration
- [ ] [T013] [P2] Add ServiceDefaults reference to API project
- [ ] [T014] [P2] Create `Directory.Build.props` for shared properties in `backend/`

### 1.2 Aspire NuGet Packages

**AppHost Project:**
- [ ] [T015] [P3] Install `Aspire.Hosting.AppHost` in BestBlogs.AppHost
- [ ] [T016] [P3] Install `Aspire.Hosting.PostgreSQL` in BestBlogs.AppHost
- [ ] [T017] [P3] Install `Aspire.Hosting.NodeJs` in BestBlogs.AppHost (for frontend)

**ServiceDefaults Project:**
- [ ] [T018] [P3] Install `Microsoft.Extensions.Http.Resilience` in BestBlogs.ServiceDefaults
- [ ] [T019] [P3] Install `Microsoft.Extensions.ServiceDiscovery` in BestBlogs.ServiceDefaults
- [ ] [T020] [P3] Install `OpenTelemetry.Exporter.OpenTelemetryProtocol` in BestBlogs.ServiceDefaults
- [ ] [T021] [P3] Install `OpenTelemetry.Extensions.Hosting` in BestBlogs.ServiceDefaults
- [ ] [T022] [P3] Install `OpenTelemetry.Instrumentation.AspNetCore` in BestBlogs.ServiceDefaults
- [ ] [T023] [P3] Install `OpenTelemetry.Instrumentation.Http` in BestBlogs.ServiceDefaults
- [ ] [T024] [P3] Install `OpenTelemetry.Instrumentation.Runtime` in BestBlogs.ServiceDefaults

**Domain Layer:**
- [ ] [T025] [P4] Install `Microsoft.Extensions.DependencyInjection.Abstractions` in BestBlogs.Domain

**Application Layer:**
- [ ] [T026] [P4] Install `FluentValidation` v11.x in BestBlogs.Application
- [ ] [T027] [P4] Install `FluentValidation.DependencyInjectionExtensions` in BestBlogs.Application
- [ ] [T028] [P4] Install `AutoMapper` v13.x in BestBlogs.Application
- [ ] [T029] [P4] Install `AutoMapper.Extensions.Microsoft.DependencyInjection` in BestBlogs.Application

**Infrastructure Layer:**
- [ ] [T030] [P4] Install `Npgsql.EntityFrameworkCore.PostgreSQL` v10.x in BestBlogs.Infrastructure
- [ ] [T031] [P4] Install `Microsoft.EntityFrameworkCore.Design` v10.x in BestBlogs.Infrastructure
- [ ] [T032] [P4] Install `Microsoft.AspNetCore.Identity.EntityFrameworkCore` v10.x in BestBlogs.Infrastructure
- [ ] [T033] [P4] Install `Microsoft.AspNetCore.Authentication.Google` v10.x in BestBlogs.Infrastructure
- [ ] [T034] [P4] Install `AspNet.Security.OAuth.GitHub` in BestBlogs.Infrastructure
- [ ] [T035] [P4] Install `Aspire.Npgsql.EntityFrameworkCore.PostgreSQL` in BestBlogs.Infrastructure

**API Layer:**
- [ ] [T036] [P4] Install `Swashbuckle.AspNetCore` v6.x in BestBlogs.API (Swagger/OpenAPI)
- [ ] [T037] [P4] Install `Serilog.AspNetCore` in BestBlogs.API
- [ ] [T038] [P4] Install `Serilog.Sinks.Console` in BestBlogs.API

**Test Projects:**
- [ ] [T039] [P4] Install `xUnit` v2.9.x in all test projects
- [ ] [T040] [P4] Install `Awesome.Assertions` in all test projects
- [ ] [T041] [P4] Install `SpecFlow.xUnit` in BestBlogs.AcceptanceTests
- [ ] [T042] [P4] Install `SpecFlow.Tools.MsBuild.Generation` in BestBlogs.AcceptanceTests
- [ ] [T043] [P4] Install `Testcontainers.PostgreSql` v3.x in BestBlogs.IntegrationTests
- [ ] [T044] [P4] Install `Microsoft.AspNetCore.Mvc.Testing` in BestBlogs.IntegrationTests

### 1.3 Frontend Project Setup

- [ ] [T045] [P5] Initialize Vite React TypeScript project in `frontend/` directory
- [ ] [T046] [P5] Install React 18, React DOM, TypeScript 5.3+
- [ ] [T047] [P5] Install React Router 6 (`react-router-dom`)
- [ ] [T048] [P5] Install React Query (`@tanstack/react-query`)
- [ ] [T049] [P5] Install Axios for HTTP client
- [ ] [T050] [P5] Install SCSS support (`sass`)
- [ ] [T051] [P5] Install Vitest and React Testing Library
- [ ] [T052] [P6] Configure TypeScript (`tsconfig.json`) with strict mode
- [ ] [T053] [P6] Configure Vite (`vite.config.ts`) with proxy for API
- [ ] [T054] [P6] Create folder structure: `features/`, `shared/`, `tests/`

---

## Phase 2: Aspire Orchestration & Foundational Infrastructure

### 2.1 .NET Aspire AppHost Configuration

- [ ] [T055] [P7] Create `Program.cs` in BestBlogs.AppHost with DistributedApplication builder
- [ ] [T056] [P7] Add PostgreSQL resource with data volume in AppHost
- [ ] [T057] [P7] Add PostgreSQL database "bestblogsdb" to Aspire configuration
- [ ] [T058] [P7] Add BestBlogs.API project reference to AppHost
- [ ] [T059] [P7] Wire up API project to PostgreSQL database connection
- [ ] [T060] [P7] Add frontend NPM app to AppHost (optional - for Aspire-managed frontend)
- [ ] [T061] [P7] Configure service discovery between API and frontend

### 2.2 Service Defaults Configuration

- [ ] [T062] [P8] Create `Extensions.cs` in BestBlogs.ServiceDefaults
- [ ] [T063] [P8] Implement `AddServiceDefaults()` extension method
- [ ] [T064] [P8] Configure OpenTelemetry with metrics (AspNetCore, HttpClient, Runtime)
- [ ] [T065] [P8] Configure OpenTelemetry with tracing (AspNetCore, HttpClient)
- [ ] [T066] [P8] Add default health checks configuration
- [ ] [T067] [P8] Add service discovery configuration
- [ ] [T068] [P9] Call `AddServiceDefaults()` in API Program.cs
- [ ] [T069] [P9] Verify Aspire dashboard runs on `dotnet run --project BestBlogs.AppHost`

### 2.3 Database Setup & EF Core Configuration

- [ ] [T070] [P10] Create `BestBlogsDbContext.cs` in `backend/src/BestBlogs.Infrastructure/Persistence/`
- [ ] [T071] [P10] Configure DbContext to use Aspire PostgreSQL connection
- [ ] [T072] [P10] Create `DbContextFactory` for design-time migrations
- [ ] [T073] [P11] Add `AddNpgsqlDbContext` with Aspire integration in Program.cs
- [ ] [T074] [P11] Configure connection string from Aspire service discovery

### 2.4 Manual CQRS Foundation (No MediatR)

- [ ] [T075] [P12] Create `ICommand<TResult>` interface in `backend/src/BestBlogs.Application/Common/`
- [ ] [T076] [P12] Create `ICommandHandler<TCommand, TResult>` interface
- [ ] [T077] [P12] Create `IQuery<TResult>` interface in `backend/src/BestBlogs.Application/Common/`
- [ ] [T078] [P12] Create `IQueryHandler<TQuery, TResult>` interface
- [ ] [T079] [P13] Create example command handler registration pattern for DI
- [ ] [T080] [P13] Document manual CQRS pattern in code comments

### 2.5 Shared Domain Foundation

- [ ] [T081] [P14] Create `IEntity` interface in `backend/src/BestBlogs.Domain/Interfaces/IEntity.cs`
- [ ] [T082] [P14] Create `BaseEntity` abstract class with Id, CreatedAt, UpdatedAt
- [ ] [T083] [P14] Create `IRepository<T>` generic interface in `backend/src/BestBlogs.Domain/Interfaces/`

### 2.6 API Middleware & Configuration

- [ ] [T084] [P15] Create `ExceptionHandlingMiddleware.cs` in `backend/src/BestBlogs.API/Middleware/`
- [ ] [T085] [P15] Create `RateLimitingMiddleware.cs` for comment spam protection
- [ ] [T086] [P15] Configure CORS policy for frontend origin in `Program.cs`
- [ ] [T087] [P15] Configure Swagger/OpenAPI in `Program.cs`
- [ ] [T088] [P16] Create `ApiResponse<T>` wrapper class for consistent responses
- [ ] [T089] [P16] Create `ErrorResponse` class for validation errors

### 2.7 Frontend API Client Setup

- [ ] [T090] [P17] Create Axios instance with base URL in `frontend/src/shared/api/client.ts`
- [ ] [T091] [P17] Configure request/response interceptors for error handling
- [ ] [T092] [P17] Create React Query client configuration in `frontend/src/shared/api/queryClient.ts`
- [ ] [T093] [P17] Wrap App component with `QueryClientProvider`

### 2.8 SCSS Design System

- [ ] [T094] [P18] Create `_variables.scss` with color palette in `frontend/src/shared/styles/`
- [ ] [T095] [P18] Create `_mixins.scss` with responsive breakpoints
- [ ] [T096] [P18] Create `global.scss` with CSS reset and base styles
- [ ] [T097] [P18] Import global styles in `frontend/src/main.tsx`

---

## Phase 3: User Story 1 - Browse and Read Blog Posts (P1)

### Tests for User Story 1 (REQUIRED per Constitution) ⚠️

**NOTE: Tests MUST be delivered with the feature (can be written before, during, or after implementation)**

- [ ] [T098] [P19] [US1] Create SpecFlow feature file `BrowseBlogPosts.feature` in `backend/tests/BestBlogs.AcceptanceTests/Features/`

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
    And I am on the homepage showing 10 posts
    When I click the "Load More" button
    Then I should see 5 additional posts
    And the "Load More" button should disappear

  Scenario: Filter posts by category
    Given the blog has 5 posts in "Travel" category
    And the blog has 3 posts in "Food" category
    When I filter by "Travel" category
    Then I should see 5 blog posts
    And all posts should be in "Travel" category

  Scenario: View single blog post
    Given a blog post exists with title "My Lifestyle Journey"
    When I view the post details
    Then I should see the post title "My Lifestyle Journey"
    And I should see the full post content
    And I should see the author name
    And I should see the published date
    And I should see the category
```

- [ ] [T099] [P20] [US1] Create step definitions `BlogSteps.cs` in `backend/tests/BestBlogs.AcceptanceTests/Steps/`
  - Use AwesomeAssertions for assertions (e.g., `posts.Count.ShouldBe(10)`)

- [ ] [T100] [P20] [US1] Create `TestContext.cs` helper in `backend/tests/BestBlogs.AcceptanceTests/Support/`
- [ ] [T101] [P20] [US1] Create integration test fixture with Testcontainers in `backend/tests/BestBlogs.IntegrationTests/TestFixture.cs`

### 3.1 Domain Layer - Blog Entities

- [ ] [T102] [P21] [US1] Create `BlogPost.cs` entity in `backend/src/BestBlogs.Domain/Entities/`
  - Properties: Id, Title, Content, Excerpt, AuthorName, CategoryId, PublishedAt, CreatedAt, UpdatedAt, IsDeleted
  - Navigation: Category, Comments (collection)

- [ ] [T103] [P21] [US1] Create `Category.cs` entity in `backend/src/BestBlogs.Domain/Entities/`
  - Properties: Id, Name, Description, CreatedAt
  - Navigation: BlogPosts (collection)
  - Invariant: "General" category cannot be deleted

- [ ] [T104] [P22] [US1] Create `PostContent` value object in `backend/src/BestBlogs.Domain/ValueObjects/`
  - Encapsulate content validation logic (min 100 chars)
  - Auto-generate excerpt if not provided (first 300 chars)

- [ ] [T105] [P22] [US1] Create `IPostRepository` interface in `backend/src/BestBlogs.Domain/Interfaces/`
  - GetPostsAsync(limit, offset, categoryId?)
  - GetPostByIdAsync(id)
  - CreatePostAsync(post)

- [ ] [T106] [P22] [US1] Create `ICategoryRepository` interface in `backend/src/BestBlogs.Domain/Interfaces/`

### 3.2 Infrastructure Layer - EF Core Configuration

- [ ] [T107] [P23] [US1] Create `BlogPostConfiguration.cs` in `backend/src/BestBlogs.Infrastructure/Persistence/Configurations/`
  - Configure table name `blog_posts`
  - Configure indexes on PublishedAt (descending)
  - Configure relationships with Category

- [ ] [T108] [P23] [US1] Create `CategoryConfiguration.cs` in `backend/src/BestBlogs.Infrastructure/Persistence/Configurations/`
  - Configure table name `categories`
  - Seed "General" category with fixed GUID

- [ ] [T109] [P24] [US1] Apply configurations to DbContext
- [ ] [T110] [P24] [US1] Create initial EF Core migration `InitialCreate`
- [ ] [T111] [P24] [US1] Create `PostRepository.cs` in `backend/src/BestBlogs.Infrastructure/Persistence/Repositories/`
- [ ] [T112] [P24] [US1] Create `CategoryRepository.cs` in `backend/src/BestBlogs.Infrastructure/Persistence/Repositories/`

### 3.3 Application Layer - Queries (Manual CQRS)

- [ ] [T113] [P25] [US1] Create `PostDto.cs` in `backend/src/BestBlogs.Application/DTOs/`
- [ ] [T114] [P25] [US1] Create `CategoryDto.cs` in `backend/src/BestBlogs.Application/DTOs/`
- [ ] [T115] [P25] [US1] Create `PostListDto.cs` with pagination metadata

- [ ] [T116] [P26] [US1] Create `GetPostsQuery.cs` in `backend/src/BestBlogs.Application/Queries/Posts/`
  - Implement `IQuery<PostListDto>`
  - Parameters: Limit (default 10), Offset (default 0), CategoryId (optional)

- [ ] [T117] [P26] [US1] Create `GetPostsQueryHandler.cs` in `backend/src/BestBlogs.Application/Queries/Posts/`
  - Implement `IQueryHandler<GetPostsQuery, PostListDto>`
  - Use AsNoTracking() for performance
  - Order by PublishedAt descending
  - Include Category navigation

- [ ] [T118] [P27] [US1] Create `GetPostByIdQuery.cs` implementing `IQuery<PostDto>`
- [ ] [T119] [P27] [US1] Create `GetPostByIdQueryHandler.cs` implementing `IQueryHandler<GetPostByIdQuery, PostDto>`

- [ ] [T120] [P28] [US1] Create `GetCategoriesQuery.cs` implementing `IQuery<List<CategoryDto>>`
- [ ] [T121] [P28] [US1] Create `GetCategoriesQueryHandler.cs` implementing `IQueryHandler<GetCategoriesQuery, List<CategoryDto>>`

- [ ] [T122] [P29] [US1] Create AutoMapper profile `MappingProfile.cs` in `backend/src/BestBlogs.Application/Mappings/`
  - Map BlogPost → PostDto
  - Map Category → CategoryDto

- [ ] [T123] [P30] [US1] Register query handlers in DI container (Program.cs)
  - `services.AddScoped<IQueryHandler<GetPostsQuery, PostListDto>, GetPostsQueryHandler>()`
  - Register all other query handlers

### 3.4 API Layer - Blog Controller

- [ ] [T124] [P31] [US1] Create `BlogController.cs` in `backend/src/BestBlogs.API/Controllers/`
  - Inject `IQueryHandler<GetPostsQuery, PostListDto>` in constructor
  - `GET /api/blog/posts?limit=10&offset=0&categoryId={guid}`
  - `GET /api/blog/posts/{id}`
  - `GET /api/blog/categories`

- [ ] [T125] [P31] [US1] Add Swagger XML comments for Blog endpoints
- [ ] [T126] [P31] [US1] Add response type annotations `[ProducesResponseType]`

### 3.5 Frontend - Blog Browsing UI

**TypeScript Types:**
- [ ] [T127] [P32] [US1] Create `post.ts` interface in `frontend/src/shared/types/`
- [ ] [T128] [P32] [US1] Create `category.ts` interface in `frontend/src/shared/types/`

**API Client:**
- [ ] [T129] [P33] [US1] Create `blog-api.ts` in `frontend/src/shared/api/`
  - `getPosts(limit, offset, categoryId?)`
  - `getPost(id)`
  - `getCategories()`

**Shared Components:**
- [ ] [T130] [P34] [US1] Create `Button.tsx` in `frontend/src/shared/components/`
- [ ] [T131] [P34] [US1] Create `Button.module.scss` with primary/secondary variants
- [ ] [T132] [P34] [US1] Create `Layout.tsx` with header/footer in `frontend/src/shared/components/`
- [ ] [T133] [P34] [US1] Create `LoadingSpinner.tsx` in `frontend/src/shared/components/`

**Blog Feature Components:**
- [ ] [T134] [P35] [US1] Create `PostCard.tsx` in `frontend/src/features/blog/components/`
  - Display title, excerpt, author, date, category
- [ ] [T135] [P35] [US1] Create `PostCard.module.scss`

- [ ] [T136] [P36] [US1] Create `PostList.tsx` in `frontend/src/features/blog/components/`
  - Render array of PostCard components
- [ ] [T137] [P36] [US1] Create `PostList.module.scss`

- [ ] [T138] [P37] [US1] Create `LoadMoreButton.tsx` in `frontend/src/features/blog/components/`
  - Hide when hasMore is false
- [ ] [T139] [P37] [US1] Create `CategoryFilter.tsx` in `frontend/src/features/blog/components/`

**React Query Hooks:**
- [ ] [T140] [P38] [US1] Create `usePosts.ts` hook in `frontend/src/features/blog/hooks/`
  - Use `useInfiniteQuery` for pagination
  - Handle loading and error states

- [ ] [T141] [P38] [US1] Create `usePost.ts` hook for single post
- [ ] [T142] [P38] [US1] Create `useCategories.ts` hook

**Pages:**
- [ ] [T143] [P39] [US1] Create `HomePage.tsx` in `frontend/src/features/blog/pages/`
  - Use usePosts hook with infinite scroll
  - Render PostList and LoadMoreButton
  - Render CategoryFilter

- [ ] [T144] [P39] [US1] Create `PostDetailPage.tsx` in `frontend/src/features/blog/pages/`
  - Use usePost hook with route param
  - Display full content with proper formatting

- [ ] [T145] [P40] [US1] Configure React Router routes in `frontend/src/router.tsx`
  - `/` → HomePage
  - `/posts/:id` → PostDetailPage

**Styling:**
- [ ] [T146] [P41] [US1] Create responsive styles for HomePage
- [ ] [T147] [P41] [US1] Create mobile-first PostCard styles with CSS Grid
- [ ] [T148] [P41] [US1] Style PostDetailPage with typography scale

### 3.6 Unit Tests for User Story 1

- [ ] [T149] [P42] [US1] Create `BlogPostTests.cs` in `backend/tests/BestBlogs.UnitTests/Domain/`
  - Test entity validation rules
  - Test PostContent value object
  - Use AwesomeAssertions (e.g., `post.Title.ShouldNotBeNullOrEmpty()`)

- [ ] [T150] [P42] [US1] Create `GetPostsQueryHandlerTests.cs` in `backend/tests/BestBlogs.UnitTests/Application/`
  - Mock IPostRepository
  - Test pagination logic
  - Test category filtering
  - Use AwesomeAssertions

### 3.7 Integration Tests for User Story 1

- [ ] [T151] [P43] [US1] Create `BlogControllerTests.cs` in `backend/tests/BestBlogs.IntegrationTests/`
  - Test GET /api/blog/posts with Testcontainers
  - Test GET /api/blog/posts/{id}
  - Test 404 response for non-existent post
  - Use AwesomeAssertions

### 3.8 Frontend Tests for User Story 1

- [ ] [T152] [P44] [US1] Create `PostCard.test.tsx` in `frontend/tests/components/`
- [ ] [T153] [P44] [US1] Create `PostList.test.tsx` in `frontend/tests/components/`
- [ ] [T154] [P44] [US1] Create `HomePage.test.tsx` in `frontend/tests/pages/`

---

## Phase 4: User Story 2 - Leave Comments on Blog Posts (P2)

### Tests for User Story 2 (REQUIRED per Constitution) ⚠️

- [ ] [T155] [P45] [US2] Create SpecFlow feature file `LeaveComments.feature` in `backend/tests/BestBlogs.AcceptanceTests/Features/`

```gherkin
Feature: Leave Comments on Blog Posts
  As a visitor
  I want to leave comments on blog posts
  So that I can engage with the content

  Scenario: Submit a valid comment
    Given a blog post exists with id "12345"
    When I submit a comment with:
      | Field          | Value                     |
      | Name           | John Doe                  |
      | Email          | john@example.com          |
      | Content        | Great post! Very helpful. |
    Then the comment should be created successfully
    And I should see my comment in the comment list

  Scenario: Reject comment with spam keywords
    Given a blog post exists with id "12345"
    When I submit a comment containing "buy viagra now"
    Then the comment should be marked as spam
    And I should not see the comment in the public list

  Scenario: Rate limit comment submissions
    Given a blog post exists with id "12345"
    And I have submitted 3 comments in the last minute
    When I try to submit another comment
    Then I should receive a "rate limit exceeded" error

  Scenario: View comments ordered by newest first
    Given a blog post has 5 comments
    When I view the post's comments
    Then I should see 5 comments ordered by date (newest first)
```

- [ ] [T156] [P46] [US2] Create step definitions `CommentSteps.cs` in `backend/tests/BestBlogs.AcceptanceTests/Steps/`
  - Use AwesomeAssertions

### 4.1 Domain Layer - Comment Entity

- [ ] [T157] [P47] [US2] Create `Comment.cs` entity in `backend/src/BestBlogs.Domain/Entities/`
  - Properties: Id, PostId, CommenterName, CommenterEmail, Content, IsSpam, CreatedAt
  - Navigation: BlogPost

- [ ] [T158] [P47] [US2] Create `Email` value object in `backend/src/BestBlogs.Domain/ValueObjects/`
  - Validate email format with regex

- [ ] [T159] [P48] [US2] Create `ICommentRepository` interface in `backend/src/BestBlogs.Domain/Interfaces/`
  - GetCommentsByPostIdAsync(postId, limit, offset)
  - CreateCommentAsync(comment)

- [ ] [T160] [P48] [US2] Create `ISpamDetectionService` interface in `backend/src/BestBlogs.Domain/Interfaces/`
  - IsSpam(content): bool

### 4.2 Domain Services - Spam Detection

- [ ] [T161] [P49] [US2] Create `SpamDetectionService.cs` in `backend/src/BestBlogs.Domain/Services/`
  - Keyword blocklist: viagra, cialis, buy now, click here, etc.
  - Case-insensitive matching
  - Return true if spam detected

- [ ] [T162] [P49] [US2] Create unit tests `SpamDetectionServiceTests.cs`
  - Test spam detection with various keywords
  - Test false positives
  - Use AwesomeAssertions

### 4.3 Infrastructure Layer - Comment Persistence

- [ ] [T163] [P50] [US2] Create `CommentConfiguration.cs` in `backend/src/BestBlogs.Infrastructure/Persistence/Configurations/`
  - Configure table name `comments`
  - Configure index on PostId and CreatedAt
  - Configure foreign key to BlogPost

- [ ] [T164] [P51] [US2] Create EF Core migration `AddComments`
- [ ] [T165] [P51] [US2] Create `CommentRepository.cs` in `backend/src/BestBlogs.Infrastructure/Persistence/Repositories/`
  - Implement GetCommentsByPostIdAsync with IsSpam filter (exclude spam)
  - Order by CreatedAt descending

- [ ] [T166] [P51] [US2] Implement `SpamFilterService.cs` in `backend/src/BestBlogs.Infrastructure/Services/`
  - Load spam keywords from configuration

### 4.4 Application Layer - Comment Commands (Manual CQRS)

- [ ] [T167] [P52] [US2] Create `CommentDto.cs` in `backend/src/BestBlogs.Application/DTOs/`
- [ ] [T168] [P52] [US2] Create `CommentListDto.cs` with pagination metadata

- [ ] [T169] [P53] [US2] Create `GetCommentsQuery.cs` implementing `IQuery<CommentListDto>`
  - Parameters: PostId, Limit (default 20), Offset (default 0)

- [ ] [T170] [P53] [US2] Create `GetCommentsQueryHandler.cs` implementing `IQueryHandler<GetCommentsQuery, CommentListDto>`
  - Exclude spam comments (IsSpam = false)

- [ ] [T171] [P54] [US2] Create `SubmitCommentCommand.cs` implementing `ICommand<Guid>`
  - Properties: PostId, CommenterName, CommenterEmail, Content

- [ ] [T172] [P54] [US2] Create `SubmitCommentCommandValidator.cs` using FluentValidation
  - Name: 2-50 chars
  - Email: valid email format
  - Content: 10-1000 chars

- [ ] [T173] [P55] [US2] Create `SubmitCommentCommandHandler.cs` implementing `ICommandHandler<SubmitCommentCommand, Guid>`
  - Check if post exists (throw NotFoundException if not)
  - Run spam detection
  - Set IsSpam flag
  - Save comment to repository
  - Return CommentDto

- [ ] [T174] [P55] [US2] Update AutoMapper profile to map Comment → CommentDto

- [ ] [T175] [P56] [US2] Register comment command/query handlers in DI container

### 4.5 API Layer - Comments Controller

- [ ] [T176] [P57] [US2] Create `CommentsController.cs` in `backend/src/BestBlogs.API/Controllers/`
  - Inject command and query handlers
  - `GET /api/comments/posts/{postId}/comments?limit=20&offset=0`
  - `POST /api/comments/posts/{postId}/comments`

- [ ] [T177] [P57] [US2] Implement rate limiting middleware for POST comments
  - Max 3 comments per IP per minute
  - Return 429 Too Many Requests

- [ ] [T178] [P57] [US2] Add Swagger annotations for Comments endpoints

### 4.6 Frontend - Comments UI

**API Client:**
- [ ] [T179] [P58] [US2] Create `comments-api.ts` in `frontend/src/shared/api/`
  - `getComments(postId, limit, offset)`
  - `submitComment(postId, data)`

**TypeScript Types:**
- [ ] [T180] [P58] [US2] Create `comment.ts` interface in `frontend/src/shared/types/`

**Comment Components:**
- [ ] [T181] [P59] [US2] Create `CommentItem.tsx` in `frontend/src/features/comments/components/`
  - Display name, content, timestamp
- [ ] [T182] [P59] [US2] Create `CommentItem.module.scss`

- [ ] [T183] [P60] [US2] Create `CommentList.tsx` in `frontend/src/features/comments/components/`
  - Render array of CommentItem
  - Show "No comments yet" if empty

- [ ] [T184] [P61] [US2] Create `CommentForm.tsx` in `frontend/src/features/comments/components/`
  - Input fields: Name, Email, Content
  - Client-side validation matching backend rules
  - Handle submission with loading state
  - Display success/error messages

- [ ] [T185] [P61] [US2] Create `Comments.module.scss` for form and list styling

**React Query Hooks:**
- [ ] [T186] [P62] [US2] Create `useComments.ts` hook in `frontend/src/features/comments/hooks/`
  - Use `useQuery` for fetching comments
  - Handle pagination with Load More

- [ ] [T187] [P62] [US2] Create `useSubmitComment.ts` hook
  - Use `useMutation` for POST
  - Invalidate comments query on success
  - Show optimistic update

**Integration with Post Detail:**
- [ ] [T188] [P63] [US2] Update `PostDetailPage.tsx` to include CommentList and CommentForm
- [ ] [T189] [P63] [US2] Add comments section styling to PostDetailPage

### 4.7 Unit Tests for User Story 2

- [ ] [T190] [P64] [US2] Create `SubmitCommentCommandHandlerTests.cs` in `backend/tests/BestBlogs.UnitTests/Application/`
  - Test spam detection integration
  - Test validation errors
  - Use AwesomeAssertions

- [ ] [T191] [P64] [US2] Create `EmailValueObjectTests.cs` in `backend/tests/BestBlogs.UnitTests/Domain/`
  - Use AwesomeAssertions

### 4.8 Integration Tests for User Story 2

- [ ] [T192] [P65] [US2] Create `CommentsControllerTests.cs` in `backend/tests/BestBlogs.IntegrationTests/`
  - Test GET comments for valid post
  - Test POST comment with valid data
  - Test POST comment with spam keywords
  - Test rate limiting behavior
  - Use AwesomeAssertions

### 4.9 Frontend Tests for User Story 2

- [ ] [T193] [P66] [US2] Create `CommentForm.test.tsx` in `frontend/tests/components/`
- [ ] [T194] [P66] [US2] Create `CommentList.test.tsx` in `frontend/tests/components/`

---

## Phase 5: User Story 3 - Manage Blog Content (P3)

### Tests for User Story 3 (REQUIRED per Constitution) ⚠️

- [ ] [T195] [P67] [US3] Create SpecFlow feature file `ManageContent.feature` in `backend/tests/BestBlogs.AcceptanceTests/Features/`

```gherkin
Feature: Manage Blog Content
  As an administrator
  I want to manage blog posts and categories
  So that I can publish and organize content

  Background:
    Given I am authenticated as an admin via OAuth

  Scenario: Create a new blog post
    When I create a blog post with:
      | Field      | Value                        |
      | Title      | My First Post                |
      | Content    | This is a long blog post...  |
      | Author     | Admin User                   |
      | CategoryId | 00000000-0000-0000-0000-001  |
    Then the post should be created successfully
    And the post should appear on the homepage

  Scenario: Update an existing blog post
    Given a blog post exists with id "12345"
    When I update the post title to "Updated Title"
    Then the post title should be "Updated Title"
    And the updatedAt timestamp should be recent

  Scenario: Delete a blog post
    Given a blog post exists with id "12345"
    When I delete the post
    Then the post should be soft-deleted
    And the post should not appear on the homepage

  Scenario: Create a new category
    When I create a category with name "Travel"
    Then the category should be created successfully
    And visitors should be able to filter by "Travel"

  Scenario: Delete a category reassigns posts to General
    Given a category "Food" exists with 5 posts
    When I delete the "Food" category
    Then all 5 posts should be reassigned to "General" category
```

- [ ] [T196] [P68] [US3] Create step definitions `AdminSteps.cs` in `backend/tests/BestBlogs.AcceptanceTests/Steps/`
  - Use AwesomeAssertions

### 5.1 Domain Layer - Admin User

- [ ] [T197] [P69] [US3] Create `AdminUser.cs` entity in `backend/src/BestBlogs.Domain/Entities/`
  - Properties: Id, Email, OAuthProvider, OAuthId, DisplayName, CreatedAt

- [ ] [T198] [P69] [US3] Create `IAdminUserRepository` interface in `backend/src/BestBlogs.Domain/Interfaces/`
  - FindByOAuthIdAsync(provider, oauthId)
  - CreateAdminUserAsync(user)

### 5.2 Infrastructure Layer - OAuth Configuration

- [ ] [T199] [P70] [US3] Create `AdminUserConfiguration.cs` in `backend/src/BestBlogs.Infrastructure/Persistence/Configurations/`
- [ ] [T200] [P71] [US3] Create EF Core migration `AddAdminUsers`

- [ ] [T201] [P72] [US3] Configure ASP.NET Core Identity in `backend/src/BestBlogs.Infrastructure/Authentication/IdentityConfiguration.cs`
  - Cookie authentication settings
  - Session lifetime: 7 days

- [ ] [T202] [P72] [US3] Create `OAuthConfiguration.cs` in `backend/src/BestBlogs.Infrastructure/Authentication/`
  - Configure Google OAuth provider
  - Configure GitHub OAuth provider
  - Set callback URLs

- [ ] [T203] [P73] [US3] Register authentication services in `Program.cs`
  - AddAuthentication()
  - AddCookie()
  - AddGoogle()
  - AddGitHub()

- [ ] [T204] [P73] [US3] Create `AdminUserRepository.cs` in `backend/src/BestBlogs.Infrastructure/Persistence/Repositories/`

### 5.3 Application Layer - Admin Commands (Manual CQRS)

**Category Management:**
- [ ] [T205] [P74] [US3] Create `CreateCategoryCommand.cs` implementing `ICommand<Guid>`
- [ ] [T206] [P74] [US3] Create `CreateCategoryCommandValidator.cs` (name 2-50 chars)
- [ ] [T207] [P74] [US3] Create `CreateCategoryCommandHandler.cs` implementing `ICommandHandler<CreateCategoryCommand, Guid>`

- [ ] [T208] [P75] [US3] Create `UpdateCategoryCommand.cs` implementing `ICommand<Unit>`
- [ ] [T209] [P75] [US3] Create `UpdateCategoryCommandHandler.cs` implementing `ICommandHandler<UpdateCategoryCommand, Unit>`

- [ ] [T210] [P76] [US3] Create `DeleteCategoryCommand.cs` implementing `ICommand<Unit>`
- [ ] [T211] [P76] [US3] Create `DeleteCategoryCommandHandler.cs` implementing `ICommandHandler<DeleteCategoryCommand, Unit>`
  - Prevent deletion of "General" category
  - Reassign posts to "General" before deletion

**Post Management:**
- [ ] [T212] [P77] [US3] Create `CreatePostCommand.cs` implementing `ICommand<Guid>`
  - Properties: Title, Content, Excerpt, AuthorName, CategoryId, PublishedAt

- [ ] [T213] [P77] [US3] Create `CreatePostCommandValidator.cs`
  - Title: 5-200 chars
  - Content: 100-50000 chars
  - Excerpt: 50-300 chars (auto-generate if not provided)
  - AuthorName: 2-100 chars

- [ ] [T214] [P78] [US3] Create `CreatePostCommandHandler.cs` implementing `ICommandHandler<CreatePostCommand, Guid>`
  - Auto-generate excerpt if not provided
  - Default to "General" category if not specified
  - Default PublishedAt to current timestamp

- [ ] [T215] [P79] [US3] Create `UpdatePostCommand.cs` implementing `ICommand<Unit>`
- [ ] [T216] [P79] [US3] Create `UpdatePostCommandHandler.cs` implementing `ICommandHandler<UpdatePostCommand, Unit>`
  - Update UpdatedAt timestamp

- [ ] [T217] [P80] [US3] Create `DeletePostCommand.cs` implementing `ICommand<Unit>`
- [ ] [T218] [P80] [US3] Create `DeletePostCommandHandler.cs` implementing `ICommandHandler<DeletePostCommand, Unit>`
  - Soft delete (set IsDeleted = true)

- [ ] [T219] [P81] [US3] Register all admin command handlers in DI container

### 5.4 API Layer - Admin Controller

- [ ] [T220] [P82] [US3] Create `AdminController.cs` in `backend/src/BestBlogs.API/Controllers/`
  - `GET /api/admin/auth/google` - Initiate Google OAuth
  - `GET /api/admin/auth/github` - Initiate GitHub OAuth
  - `GET /api/admin/auth/callback` - OAuth callback
  - `POST /api/admin/auth/logout` - Logout

- [ ] [T221] [P83] [US3] Add `[Authorize]` attribute to protected endpoints
- [ ] [T222] [P83] [US3] Implement OAuth callback handler
  - Create AdminUser if first login
  - Set authentication cookie

- [ ] [T223] [P84] [US3] Add admin endpoints to AdminController:
  - Inject all required command handlers
  - `POST /api/admin/posts` - Create post
  - `PUT /api/admin/posts/{id}` - Update post
  - `DELETE /api/admin/posts/{id}` - Delete post
  - `POST /api/admin/categories` - Create category
  - `PUT /api/admin/categories/{id}` - Update category
  - `DELETE /api/admin/categories/{id}` - Delete category

- [ ] [T224] [P84] [US3] Add Swagger annotations with OAuth security scheme

### 5.5 Frontend - Admin Interface

**Shared Auth Hook:**
- [ ] [T225] [P85] [US3] Create `useAuth.ts` hook in `frontend/src/shared/hooks/`
  - Check authentication status
  - Handle logout

**API Client:**
- [ ] [T226] [P86] [US3] Create `admin-api.ts` in `frontend/src/shared/api/`
  - `createPost(data)`
  - `updatePost(id, data)`
  - `deletePost(id)`
  - `createCategory(data)`
  - `updateCategory(id, data)`
  - `deleteCategory(id)`
  - `logout()`

**Admin Components:**
- [ ] [T227] [P87] [US3] Create `LoginPage.tsx` in `frontend/src/features/admin/pages/`
  - Google OAuth button
  - GitHub OAuth button
  - Redirect to `/api/admin/auth/google` and `/api/admin/auth/github`

- [ ] [T228] [P88] [US3] Create `PostEditor.tsx` in `frontend/src/features/admin/components/`
  - Form with Title, Content, Excerpt, Author, Category, PublishedAt
  - Rich text editor for Content (simple textarea for MVP)
  - Client-side validation

- [ ] [T229] [P89] [US3] Create `PostManagement.tsx` in `frontend/src/features/admin/components/`
  - Table listing all posts
  - Edit and Delete buttons
  - Filter by category

- [ ] [T230] [P90] [US3] Create `CategoryManager.tsx` in `frontend/src/features/admin/components/`
  - List all categories with post counts
  - Create new category form
  - Edit/Delete category buttons
  - Warning when deleting category with posts

- [ ] [T231] [P91] [US3] Create `AdminDashboard.tsx` in `frontend/src/features/admin/pages/`
  - Tab navigation: Posts, Categories
  - Show PostManagement and CategoryManager

- [ ] [T232] [P92] [US3] Create `Admin.module.scss` for admin interface styling
  - Dashboard layout with sidebar
  - Form styling
  - Table styling

**React Query Hooks:**
- [ ] [T233] [P93] [US3] Create `useCreatePost.ts` mutation hook
- [ ] [T234] [P93] [US3] Create `useUpdatePost.ts` mutation hook
- [ ] [T235] [P93] [US3] Create `useDeletePost.ts` mutation hook
- [ ] [T236] [P94] [US3] Create `useCreateCategory.ts` mutation hook
- [ ] [T237] [P94] [US3] Create `useUpdateCategory.ts` mutation hook
- [ ] [T238] [P94] [US3] Create `useDeleteCategory.ts` mutation hook

**Routing:**
- [ ] [T239] [P95] [US3] Add admin routes to `frontend/src/router.tsx`
  - `/admin/login` → LoginPage (public)
  - `/admin` → AdminDashboard (protected route)

- [ ] [T240] [P95] [US3] Create `ProtectedRoute` component to check authentication
  - Redirect to `/admin/login` if not authenticated

### 5.6 Unit Tests for User Story 3

- [ ] [T241] [P96] [US3] Create `CreatePostCommandHandlerTests.cs` in `backend/tests/BestBlogs.UnitTests/Application/`
  - Test auto-generation of excerpt
  - Test default category assignment
  - Use AwesomeAssertions

- [ ] [T242] [P96] [US3] Create `DeleteCategoryCommandHandlerTests.cs`
  - Test reassignment to "General" category
  - Test prevention of deleting "General"
  - Use AwesomeAssertions

### 5.7 Integration Tests for User Story 3

- [ ] [T243] [P97] [US3] Create `AdminControllerTests.cs` in `backend/tests/BestBlogs.IntegrationTests/`
  - Test OAuth callback flow (mock OAuth provider)
  - Test POST /api/admin/posts with authentication
  - Test 401 response for unauthenticated requests
  - Test CRUD operations for posts and categories
  - Use AwesomeAssertions

### 5.8 Frontend Tests for User Story 3

- [ ] [T244] [P98] [US3] Create `PostEditor.test.tsx` in `frontend/tests/components/`
- [ ] [T245] [P98] [US3] Create `CategoryManager.test.tsx` in `frontend/tests/components/`
- [ ] [T246] [P98] [US3] Create `AdminDashboard.test.tsx` in `frontend/tests/pages/`

---

## Phase 6: GitHub Actions CI/CD

### 6.1 Backend CI Workflow

- [ ] [T247] [P99] Create `.github/workflows/backend-ci.yml`
  - Trigger on changes to `backend/**`
  - Use .NET 10 (`dotnet-version: '10.0.x'`)
  - Restore dependencies with `dotnet restore`
  - Build with `dotnet build --no-restore`
  - Run unit tests with `dotnet test BestBlogs.UnitTests`
  - Run integration tests with `dotnet test BestBlogs.IntegrationTests`
  - Run acceptance tests with `dotnet test BestBlogs.AcceptanceTests`
  - Upload test results as artifacts

- [ ] [T248] [P99] Configure Docker service for Testcontainers in GitHub Actions
- [ ] [T249] [P100] Add code coverage reporting with Coverlet

### 6.2 Frontend CI Workflow

- [ ] [T250] [P101] Create `.github/workflows/frontend-ci.yml`
  - Trigger on changes to `frontend/**`
  - Install dependencies with `npm ci`
  - Run linting with `npm run lint`
  - Run component tests with `npm test`
  - Build production bundle with `npm run build`
  - Upload build artifacts

### 6.3 Deployment Workflow

- [ ] [T251] [P102] Create `.github/workflows/deploy.yml`
  - Trigger on push to main branch
  - Build Docker image for backend
  - Build Docker image for frontend
  - Push images to container registry
  - Deploy to hosting platform (Azure/AWS)
  - Run smoke tests

- [ ] [T252] [P102] Create `Dockerfile` for backend in `backend/`
- [ ] [T253] [P102] Create `Dockerfile` for frontend in `frontend/`
- [ ] [T254] [P103] Create Aspire deployment manifest
  - Use `dotnet run --project BestBlogs.AppHost -- --publisher manifest`
  - Generate deployment descriptors for production

---

## Phase 7: Polish & Documentation

### 7.1 API Documentation

- [ ] [T255] [P104] Ensure all API endpoints have XML comments
- [ ] [T256] [P104] Configure Swagger UI with OAuth authentication flow
- [ ] [T257] [P104] Add API examples to Swagger documentation

### 7.2 Error Handling & Validation

- [ ] [T258] [P105] Verify all validation messages are user-friendly
- [ ] [T259] [P105] Ensure consistent error response format across all endpoints
- [ ] [T260] [P105] Add global exception handler for unhandled exceptions

### 7.3 Performance Optimization

- [ ] [T261] [P106] Add EF Core query logging in development
- [ ] [T262] [P106] Verify all queries use AsNoTracking() where appropriate
- [ ] [T263] [P106] Add indexes to frequently queried columns
- [ ] [T264] [P107] Configure PostgreSQL connection pooling via Aspire
- [ ] [T265] [P107] Add response compression middleware

### 7.4 Security Hardening

- [ ] [T266] [P108] Configure HTTPS enforcement in production
- [ ] [T267] [P108] Add security headers middleware (HSTS, CSP, X-Frame-Options)
- [ ] [T268] [P108] Configure CORS to allow only frontend origin
- [ ] [T269] [P109] Add anti-forgery token validation for admin endpoints
- [ ] [T270] [P109] Configure secure cookie settings (HttpOnly, Secure, SameSite)

### 7.5 Logging & Observability

- [ ] [T271] [P110] Configure Serilog with structured logging
- [ ] [T272] [P110] Add request/response logging middleware
- [ ] [T273] [P110] Configure log levels for different environments
- [ ] [T274] [P111] Verify Aspire OpenTelemetry exports to dashboard
- [ ] [T275] [P111] Configure production telemetry export (Azure Monitor/AWS X-Ray)

### 7.6 Developer Documentation

- [ ] [T276] [P112] Update `quickstart.md` with Aspire workflow
  - Document `dotnet run --project BestBlogs.AppHost` command
  - Document Aspire dashboard at http://localhost:15000

- [ ] [T277] [P112] Document OAuth setup for Google and GitHub
- [ ] [T278] [P112] Document database migration workflow
- [ ] [T279] [P113] Create API usage examples in `specs/001-blog-platform/examples/`
- [ ] [T280] [P113] Document deployment process with Aspire manifests

### 7.7 Final Testing & Validation

- [ ] [T281] [P114] Run all SpecFlow acceptance tests and verify they pass
- [ ] [T282] [P114] Run full integration test suite with Testcontainers
- [ ] [T283] [P114] Verify all frontend component tests pass
- [ ] [T284] [P115] Manual testing of complete user flows:
  - Browse posts → View post → Submit comment
  - Admin login → Create post → Edit post → Delete post
  - Category creation and filtering

- [ ] [T285] [P116] Load testing with sample data (100 posts, 500 comments)
- [ ] [T286] [P116] Verify performance goals:
  - API response time < 200ms p95
  - Homepage load < 2 seconds
  - Post detail page < 3 seconds

- [ ] [T287] [P117] Verify Aspire dashboard shows all services healthy
- [ ] [T288] [P117] Test deployment manifest generation for production

---

## Summary

**Total Tasks**: 288 tasks across 7 phases

**Parallelization**: Tasks marked with [P] numbers can run in parallel within their phase.

**User Story Mapping**:
- **[US1]**: Browse and Read Blog Posts (Tasks T098-T154) - 57 tasks
- **[US2]**: Leave Comments (Tasks T155-T194) - 40 tasks
- **[US3]**: Manage Blog Content (Tasks T195-T246) - 52 tasks
- **Infrastructure**: Setup, Aspire, Manual CQRS, CI/CD, Polish (Tasks T001-T097, T247-T288) - 139 tasks

**Test Coverage**:
- 3 SpecFlow feature files with Gherkin scenarios (acceptance tests)
- Integration tests with Testcontainers (real PostgreSQL)
- Unit tests for domain logic and command/query handlers
- Frontend component tests with Vitest
- **All tests use AwesomeAssertions**

**Key Technology Stack**:
- .NET 10 with C# 13
- .NET Aspire for orchestration and observability
- Manual CQRS pattern (no MediatR library)
- Entity Framework Core 10 with PostgreSQL 16
- AwesomeAssertions for readable test assertions
- React 18 + TypeScript frontend

**Dependencies**:
- Phase 1-2 must complete before Phase 3 (Aspire + foundational setup)
- Phase 3 (US1) can run independently after Phase 2
- Phase 4 (US2) depends on Phase 3 (needs BlogPost entity and API)
- Phase 5 (US3) can start after Phase 2 (infrastructure ready)
- Phase 6 (CI/CD) can run in parallel with Phase 3-5
- Phase 7 (Polish) runs after all features complete

**Constitution Compliance**:
✅ Modular Monolith Architecture (DDD layers + Aspire orchestration)
✅ Test-Required Development (SpecFlow + xUnit + AwesomeAssertions)
✅ Simplicity & YAGNI (Manual CQRS, Aspire simplifies orchestration, keyword spam filter)

---

## Next Steps

1. Review this task list with the team
2. Set up development environment with .NET 10 and Aspire
3. Begin with Phase 1: Solution Setup (T001-T054)
4. Run Aspire AppHost to verify orchestration: `dotnet run --project BestBlogs.AppHost`
5. Follow BDD approach: Write Gherkin feature files first for each user story
6. Implement features in priority order: US1 → US2 → US3
7. Deliver tests with each feature per Test-Required principle

**Aspire Development Workflow**:
```bash
# Phase 1-2: Setup
cd backend
dotnet new sln -n BestBlogs
dotnet new aspire-apphost -n BestBlogs.AppHost
dotnet new aspire-servicedefaults -n BestBlogs.ServiceDefaults
dotnet new classlib -n BestBlogs.Domain
# ... etc.

# Phase 3+: Development
cd backend/src/BestBlogs.AppHost
dotnet run

# Aspire dashboard: http://localhost:15000
# API: http://localhost:5000 (auto-configured by Aspire)
# Frontend: http://localhost:5173 (Aspire-managed or standalone)
```

**Manual CQRS Pattern Example**:
```csharp
// No MediatR - direct handler injection
public class BlogController : ControllerBase
{
    private readonly IQueryHandler<GetPostsQuery, PostListDto> _getPostsHandler;

    public BlogController(IQueryHandler<GetPostsQuery, PostListDto> getPostsHandler)
    {
        _getPostsHandler = getPostsHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        var query = new GetPostsQuery(limit, offset);
        var result = await _getPostsHandler.HandleAsync(query);
        return Ok(result);
    }
}
```
