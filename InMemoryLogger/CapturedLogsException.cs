namespace InMemoryLogger;

/// <summary>
/// Исключение, содержащее информацию о собранных логах
/// с деталями внутренних исключений.
/// </summary>
public sealed class CapturedLogsException: Exception
{
    /// <summary>
    /// Создаёт экземпляр <see cref="CapturedLogsException"/>,
    /// формируя сообщение из коллекции логов.
    /// </summary>
    /// <param name="logs">
    /// Коллекция логов, из которых извлекаются сообщения об исключениях.
    /// </param>
    public CapturedLogsException(IEnumerable<InMemoryLogEntry> logs) : base(BuildMessage(logs)) { }

    /// <summary>
    /// Проверяет коллекцию логов и выбрасывает <see cref="CapturedLogsException"/>,
    /// если хотя бы один элемент содержит исключение.
    /// </summary>
    /// <param name="logs">Коллекция логов для проверки.</param>
    /// <exception cref="ArgumentNullException">
    /// Если параметр <paramref name="logs"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="CapturedLogsException">
    /// Если в коллекции найден хотя бы один элемент с исключением.
    /// </exception>
    public static void ThrowIfContainsException(IEnumerable<InMemoryLogEntry> logs)
    {
        ArgumentNullException.ThrowIfNull(logs);

        if (logs.Any(m => m.Exception is not null))
        {
            throw new CapturedLogsException(logs);
        }
    }

    private static string BuildMessage(IEnumerable<InMemoryLogEntry> logs)
    {
        ArgumentNullException.ThrowIfNull(logs);

        var exceptionLogs = logs
            .Where(m => m.Exception is not null)
            .Select(FormatLog);

        return exceptionLogs.Any()
            ? string.Join(Environment.NewLine + Environment.NewLine, exceptionLogs)
            : "no exceptions in the logs";
    }

    private static string FormatLog(InMemoryLogEntry log) =>
        string.Join(
            Environment.NewLine,
            log.Message,
            log.Exception?.Message,
            log.Exception?.StackTrace);
}
