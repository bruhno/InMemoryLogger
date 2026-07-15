using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace InMemoryLogger;

/// <summary>
/// In-memory logger provider for testing.
/// </summary>
/// <remarks>
/// Stores all log entries in a thread-safe collection.
/// </remarks>
public sealed class InMemoryLoggerProvider: ILoggerProvider
{
    /// <summary>
    /// Gets all captured log entries.
    /// </summary>
    public IReadOnlyCollection<InMemoryLogEntry> LogEntries => [.. _logs];

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryLoggerProvider"/> class.
    /// </summary>
    public InMemoryLoggerProvider() : this([]) { }

    /// <summary>
    /// Throws if any logged exception is found.
    /// </summary>
    /// <exception cref="CapturedLogsException">
    /// One or more log entries contain an exception.
    /// </exception>
    public void EnsureNoExceptions() =>
        CapturedLogsException.ThrowIfContainsException(_logs);

    /// <summary>
    /// Throws if any logged exception is not one of the specified types.
    /// </summary>
    /// <param name="exceptionTypes">
    /// Exception types to ignore.
    /// </param>
    /// <exception cref="CapturedLogsException">
    /// One or more unexpected exceptions were found.
    /// </exception>
    public void EnsureNoExceptionsExcept(params Type[] exceptionTypes)
    {
        ArgumentNullException.ThrowIfNull(exceptionTypes);

        var logsWithExceptions = _logs
            .Where(entry => 
                entry.Exception is not null
                &&  !exceptionTypes.Any(t=>t.IsAssignableFrom(entry.Exception.GetType())))
            .ToList();            

        CapturedLogsException.ThrowIfContainsException(logsWithExceptions);
    }

    /// <summary>
    /// Throws if any logged exception except cancellation exceptions is found.
    /// </summary>
    public void EnsureNoExceptionsExceptCancellation() =>
        EnsureNoExceptionsExcept(typeof(OperationCanceledException));

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) => new InMemoryLogger(_logs);

    /// <inheritdoc />
    public void Dispose() { }


    private InMemoryLoggerProvider(ConcurrentBag<InMemoryLogEntry> logs)
    {
        ArgumentNullException.ThrowIfNull(logs);

        _logs = logs;
    }

    private readonly ConcurrentBag<InMemoryLogEntry> _logs;
}
