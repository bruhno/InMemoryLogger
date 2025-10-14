using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace InMemoryLogger;

/// <summary>
/// Потокобезопасный провайдер логов, сохраняющий все записи в памяти.
/// </summary>
/// <remarks>
/// Подходит для тестирования и отладки, когда ошибки не выбрасываются напрямую,
/// а записываются только в лог.
/// </remarks>
public sealed class InMemoryLoggerProvider: ILoggerProvider
{
    /// <summary>
    /// Возвращает все записи логов, накопленные данным провайдером.
    /// </summary>
    public IReadOnlyCollection<InMemoryLogEntry> LogEntries => [.. _logs];

    /// <summary>
    /// Создаёт новый экземпляр <see cref="InMemoryLoggerProvider"/> 
    /// с пустой потокобезопасной коллекцией логов.
    /// </summary>
    public InMemoryLoggerProvider() : this([]) { }

    /// <summary>
    /// Проверяет, содержит ли журнал логов исключения, 
    /// и выбрасывает первое найденное.
    /// </summary>
    /// <remarks>
    /// Использует метод <see cref="CapturedLogsException.ThrowIfContainsException(IEnumerable{InMemoryLogEntry})"/>.
    /// Полезен при тестировании: позволяет убедиться, что в ходе выполнения
    /// не было зафиксировано ошибок.
    /// </remarks>
    /// <exception cref="Exception">
    /// Выбрасывается, если хотя бы одно исключение присутствует среди логов.
    /// </exception>
    public void EnsureNoExceptions() =>
        CapturedLogsException.ThrowIfContainsException(_logs);

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) => new InMemoryLogger(_logs);

    /// <inheritdoc />
    public void Dispose() { }

    /// <summary>
    /// Инициализирует провайдер с указанной коллекцией логов.
    /// </summary>
    /// <param name="logs">
    /// Потокобезопасная коллекция, в которую будут записываться логи.
    /// </param>
    private InMemoryLoggerProvider(ConcurrentBag<InMemoryLogEntry> logs)
    {
        _logs = logs ?? throw new ArgumentNullException(nameof(logs));
    }

    private readonly ConcurrentBag<InMemoryLogEntry> _logs;
}
