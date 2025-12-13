using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Awesome.Assertions;
using BestBlogs.Application.DTOs;
using BestBlogs.AcceptanceTests.Support;
using TechTalk.SpecFlow;

namespace BestBlogs.AcceptanceTests.Steps;

[Binding]
public class BlogSteps
{
    private readonly TestContext _context;
    private HttpResponseMessage? _response;
    private PostListDto? _postList;
    private PostDto? _singlePost;

    public BlogSteps(TestContext context)
    {
        _context = context;
    }

    [Given(@"the blog has (\d+) published posts")]
    public async Task GivenTheBlogHasPublishedPosts(int count)
    {
        await _context.SeedBlogPostsAsync(count);
    }

    [Given(@"I am on the homepage showing (\d+) posts")]
    public async Task GivenIAmOnTheHomepageShowingPosts(int count)
    {
        _response = await _context.Client.GetAsync("/api/blog/posts?limit=10&offset=0");
        _postList = await _response.Content.ReadFromJsonAsync<PostListDto>();
        _postList.ShouldNotBeNull();
        _postList.Posts.Count.ShouldBe(count);
    }

    [Given(@"the blog has (\d+) posts in ""([^""]*)"" category")]
    public async Task GivenTheBlogHasPostsInCategory(int count, string categoryName)
    {
        await _context.SeedBlogPostsInCategoryAsync(count, categoryName);
    }

    [Given(@"a blog post exists with title ""([^""]*)""")]
    public async Task GivenABlogPostExistsWithTitle(string title)
    {
        await _context.SeedBlogPostWithTitleAsync(title);
    }

    [When(@"I visit the homepage")]
    public async Task WhenIVisitTheHomepage()
    {
        _response = await _context.Client.GetAsync("/api/blog/posts?limit=10&offset=0");
        _response.EnsureSuccessStatusCode();
        _postList = await _response.Content.ReadFromJsonAsync<PostListDto>();
    }

    [When(@"I click the ""Load More"" button")]
    public async Task WhenIClickTheLoadMoreButton()
    {
        _response = await _context.Client.GetAsync("/api/blog/posts?limit=10&offset=10");
        _response.EnsureSuccessStatusCode();
        var nextPage = await _response.Content.ReadFromJsonAsync<PostListDto>();

        // Simulate appending to existing list
        if (_postList != null && nextPage != null)
        {
            _postList.Posts.AddRange(nextPage.Posts);
            _postList.HasMore = nextPage.HasMore;
            _postList.Offset = nextPage.Offset;
        }
    }

    [When(@"I filter by ""([^""]*)"" category")]
    public async Task WhenIFilterByCategory(string categoryName)
    {
        var categoryId = await _context.GetCategoryIdByNameAsync(categoryName);
        _response = await _context.Client.GetAsync($"/api/blog/posts?categoryId={categoryId}");
        _response.EnsureSuccessStatusCode();
        _postList = await _response.Content.ReadFromJsonAsync<PostListDto>();
    }

    [When(@"I view the post details")]
    public async Task WhenIViewThePostDetails()
    {
        var postId = await _context.GetPostIdByTitleAsync("My Lifestyle Journey");
        _response = await _context.Client.GetAsync($"/api/blog/posts/{postId}");
        _response.EnsureSuccessStatusCode();
        _singlePost = await _response.Content.ReadFromJsonAsync<PostDto>();
    }

    [Then(@"I should see (\d+) blog posts")]
    public void ThenIShouldSeeBlogPosts(int expectedCount)
    {
        _postList.ShouldNotBeNull();
        _postList.Posts.Count.ShouldBe(expectedCount);
    }

    [Then(@"I should see a ""Load More"" button")]
    public void ThenIShouldSeeALoadMoreButton()
    {
        _postList.ShouldNotBeNull();
        _postList.HasMore.ShouldBeTrue();
    }

    [Then(@"the posts should be ordered by published date \(newest first\)")]
    public void ThenThePostsShouldBeOrderedByPublishedDateNewestFirst()
    {
        _postList.ShouldNotBeNull();
        _postList.Posts.Count.ShouldBeGreaterThan(0);

        for (int i = 0; i < _postList.Posts.Count - 1; i++)
        {
            var current = _postList.Posts[i].PublishedAt;
            var next = _postList.Posts[i + 1].PublishedAt;
            (current >= next).ShouldBeTrue($"Post at index {i} should be newer than or equal to post at index {i + 1}");
        }
    }

    [Then(@"I should see (\d+) additional posts")]
    public void ThenIShouldSeeAdditionalPosts(int additionalCount)
    {
        _postList.ShouldNotBeNull();
        _postList.Posts.Count.ShouldBe(15); // 10 initial + 5 additional
    }

    [Then(@"the ""Load More"" button should disappear")]
    public void ThenTheLoadMoreButtonShouldDisappear()
    {
        _postList.ShouldNotBeNull();
        _postList.HasMore.ShouldBeFalse();
    }

    [Then(@"all posts should be in ""([^""]*)"" category")]
    public void ThenAllPostsShouldBeInCategory(string categoryName)
    {
        _postList.ShouldNotBeNull();
        _postList.Posts.Count.ShouldBeGreaterThan(0);

        foreach (var post in _postList.Posts)
        {
            post.Category.Name.ShouldBe(categoryName);
        }
    }

    [Then(@"I should see the post title ""([^""]*)""")]
    public void ThenIShouldSeeThePostTitle(string expectedTitle)
    {
        _singlePost.ShouldNotBeNull();
        _singlePost.Title.ShouldBe(expectedTitle);
    }

    [Then(@"I should see the full post content")]
    public void ThenIShouldSeeTheFullPostContent()
    {
        _singlePost.ShouldNotBeNull();
        _singlePost.Content.ShouldNotBeNullOrEmpty();
        _singlePost.Content.Length.ShouldBeGreaterThan(100);
    }

    [Then(@"I should see the author name")]
    public void ThenIShouldSeeTheAuthorName()
    {
        _singlePost.ShouldNotBeNull();
        _singlePost.AuthorName.ShouldNotBeNullOrEmpty();
    }

    [Then(@"I should see the published date")]
    public void ThenIShouldSeeThePublishedDate()
    {
        _singlePost.ShouldNotBeNull();
        _singlePost.PublishedAt.ShouldBeGreaterThan(DateTime.MinValue);
    }

    [Then(@"I should see the category")]
    public void ThenIShouldSeeTheCategory()
    {
        _singlePost.ShouldNotBeNull();
        _singlePost.Category.ShouldNotBeNull();
        _singlePost.Category.Name.ShouldNotBeNullOrEmpty();
    }
}
