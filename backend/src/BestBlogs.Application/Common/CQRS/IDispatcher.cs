namespace BestBlogs.Application.Common.CQRS;

/// <summary>
/// Dispatcher for sending commands and queries to their respective handlers
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// Sends a command that doesn't return a result
    /// </summary>
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand;

    /// <summary>
    /// Sends a command that returns a result
    /// </summary>
    Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResult>;

    /// <summary>
    /// Sends a query that returns data
    /// </summary>
    Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>;
}
