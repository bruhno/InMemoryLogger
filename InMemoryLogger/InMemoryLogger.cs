using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace InMemoryLogger;

internal sealed class InMemoryLogger(ConcurrentBag<InMemoryLogEntry> _logs): ILogger
{
    public IDisposable? BeginScope<TState>(TState _) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        _logs.Add(new InMemoryLogEntry
        {
            Level = logLevel,
            Message = formatter(state, exception),
            Exception = exception,
        });

    //private readonly ConcurrentBag<InMemoryLogEntry> _logs = logs ?? throw new ArgumentNullException(nameof(logs));
}
