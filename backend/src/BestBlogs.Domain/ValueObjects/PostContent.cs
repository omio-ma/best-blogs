using System;
using System.Linq;

namespace BestBlogs.Domain.ValueObjects;

/// <summary>
/// Value object encapsulating blog post content with validation and excerpt generation
/// </summary>
public class PostContent
{
    private const int MinContentLength = 100;
    private const int MaxContentLength = 50000;
    private const int ExcerptLength = 300;

    public string Content { get; }
    public string Excerpt { get; }

    private PostContent(string content, string? excerpt = null)
    {
        Content = content;
        Excerpt = excerpt ?? GenerateExcerpt(content);
    }

    /// <summary>
    /// Creates a PostContent with validated content and auto-generated or custom excerpt
    /// </summary>
    public static PostContent Create(string content, string? excerpt = null)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be empty", nameof(content));
        }

        if (content.Length < MinContentLength)
        {
            throw new ArgumentException($"Content must be at least {MinContentLength} characters", nameof(content));
        }

        if (content.Length > MaxContentLength)
        {
            throw new ArgumentException($"Content cannot exceed {MaxContentLength} characters", nameof(content));
        }

        // Validate custom excerpt if provided
        if (excerpt != null)
        {
            if (string.IsNullOrWhiteSpace(excerpt))
            {
                throw new ArgumentException("Excerpt cannot be empty if provided", nameof(excerpt));
            }

            if (excerpt.Length > ExcerptLength)
            {
                throw new ArgumentException($"Excerpt cannot exceed {ExcerptLength} characters", nameof(excerpt));
            }
        }

        return new PostContent(content, excerpt);
    }

    /// <summary>
    /// Generates an excerpt from the first 300 characters of content
    /// </summary>
    private static string GenerateExcerpt(string content)
    {
        if (content.Length <= ExcerptLength)
        {
            return content;
        }

        // Take first 300 chars and try to end at a word boundary
        var excerpt = content.Substring(0, ExcerptLength);
        var lastSpaceIndex = excerpt.LastIndexOf(' ');

        if (lastSpaceIndex > 0)
        {
            excerpt = excerpt.Substring(0, lastSpaceIndex);
        }

        return excerpt + "...";
    }
}
