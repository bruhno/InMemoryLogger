using Microsoft.Extensions.Logging;

namespace InMemoryLogger;

/// <summary>
/// Представляет собой одну запись лога.
/// </summary>
public sealed class InMemoryLogEntry
{
    /// <summary>
    /// Уровень логирования записи.
    /// </summary>
    public required LogLevel Level { get; init; }

    /// <summary>
    /// Текст сообщения.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Исключение, связанное с записью (если есть).
    /// </summary>
    public required Exception? Exception { get; init; }
}