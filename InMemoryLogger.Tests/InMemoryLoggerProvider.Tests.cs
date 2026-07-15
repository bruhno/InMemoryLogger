using FluentAssertions;

using Microsoft.Extensions.Logging;

namespace InMemoryLogger.Tests;

public sealed class InMemoryLoggerProviderTests
{
    [Fact]
    public void EnsureNoExceptions_NoLogs_DoesNotThrow()
    {
        // Arrange
        var provider = new InMemoryLoggerProvider();

        // Act
        Action act = provider.EnsureNoExceptions;

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureNoExceptions_NoExceptions_DoesNotThrow()
    {
        // Arrange
        var provider = new InMemoryLoggerProvider();
        var logger = provider.CreateLogger("Tests");

        logger.LogInformation("Everything is fine");

        // Act
        Action act = provider.EnsureNoExceptions;

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureNoExceptions_ExceptionLogged_ThrowsCapturedLogsException()
    {
        // Arrange
        var provider = new InMemoryLoggerProvider();
        var logger = provider.CreateLogger("Tests");

        logger.LogError(new InvalidOperationException("Boom"), "Unexpected error");

        // Act
        Action act = provider.EnsureNoExceptions;

        // Assert
        act.Should()
            .Throw<CapturedLogsException>()
            .WithMessage("*Boom*");
    }

    [Fact]
    public void EnsureNoExceptionsExcept_AllowedException_DoesNotThrow()
    {
        // Arrange
        var provider = new InMemoryLoggerProvider();
        var logger = provider.CreateLogger("Tests");

        logger.LogError(new InvalidOperationException(), "Unexpected error");

        // Act
        Action act = () => provider.EnsureNoExceptionsExcept(typeof(InvalidOperationException));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureNoExceptionsExcept_DerivedException_DoesNotThrow()
    {
        // Arrange
        var provider = new InMemoryLoggerProvider();
        var logger = provider.CreateLogger("Tests");

        logger.LogError(new FileNotFoundException(), "Unexpected error");

        // Act
        Action act = () => provider.EnsureNoExceptionsExcept(typeof(IOException));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureNoExceptionsExcept_UnexpectedException_ThrowsCapturedLogsException()
    {
        // Arrange
        var provider = new InMemoryLoggerProvider();
        var logger = provider.CreateLogger("Tests");

        logger.LogError(new InvalidOperationException(), "Unexpected error");

        // Act
        Action act = () => provider.EnsureNoExceptionsExcept(typeof(ArgumentException));

        // Assert
        act.Should().Throw<CapturedLogsException>();
    }

    [Fact]
    public void EnsureNoExceptionsExcept_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var provider = new InMemoryLoggerProvider();

        // Act
        Action act = () => provider.EnsureNoExceptionsExcept(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LogEntries_ReturnsCapturedEntries()
    {
        // Arrange
        var provider = new InMemoryLoggerProvider();
        var logger = provider.CreateLogger("Tests");

        logger.LogInformation("First");
        logger.LogError(new InvalidOperationException(), "Second");

        // Act
        var entries = provider.LogEntries;

        // Assert
        entries.Should().HaveCount(2);
    }
}