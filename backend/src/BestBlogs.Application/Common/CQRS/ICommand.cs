namespace BestBlogs.Application.Common.CQRS;

/// <summary>
/// Marker interface for commands that perform actions and don't return data
/// </summary>
public interface ICommand
{
}

/// <summary>
/// Interface for commands that return a result
/// </summary>
/// <typeparam name="TResult">The type of result returned by the command</typeparam>
public interface ICommand<out TResult>
{
}
