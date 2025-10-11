using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

namespace InMemoryLogger;

/// <summary>
/// Простейшая реализация <see cref="ILoggerProvider"/>, 
/// которая сохраняет записи логов в памяти.
/// </summary>
public sealed partial class InMemoryLoggerProvider : ILoggerProvider
{
    /// <summary>
    /// Создаёт новый экземпляр <see cref="InMemoryLoggerProvider"/>.
    /// </summary>
    /// <param name="logs">
    /// Потокобезопасная коллекция для хранения логов.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="logs"/> равен <c>null</c>.
    /// </exception>
    public InMemoryLoggerProvider(ConcurrentBag<InMemoryLogEntry> logs)
    {
        _logs = logs ?? throw new ArgumentNullException(nameof(logs));
    }

    /// <summary>
    /// Создаёт новый экземпляр <see cref="ILogger"/>.
    /// </summary>
    /// <param name="categoryName">
    /// Имя категории логгера. В данной реализации не используется.
    /// </param>
    /// <returns>Новый экземпляр <see cref="ILogger"/>.</returns>
    public ILogger CreateLogger(string categoryName) => new InMemoryLogger(_logs);

    /// <summary>
    /// Освобождает ресурсы, занятые данным провайдером логов.
    /// В данной реализации освобождать нечего.
    /// </summary>
    public void Dispose() { }

    private readonly ConcurrentBag<InMemoryLogEntry> _logs;
}