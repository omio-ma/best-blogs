namespace BestBlogs.Application.Common.CQRS;

/// <summary>
/// Interface for queries that return data without modifying state
/// </summary>
/// <typeparam name="TResult">The type of result returned by the query</typeparam>
public interface IQuery<out TResult>
{
}
