# InMemoryLogger

🧩 Lightweight in-memory logging provider for .NET integration and unit testing.  
Collect, inspect, and assert logs without external dependencies.

---

## ✨ Features

- 🧠 Implements `ILoggerProvider` and `ILogger` — fully compatible with `Microsoft.Extensions.Logging`
- 💾 Stores all logs in-memory (`ConcurrentBag<InMemoryLogEntry>`)
- 🧪 Designed for use in unit and integration tests
- 🚨 Includes `CapturedLogsException` for aggregating log exceptions into a single error
- 🔍 Simple, dependency-free, and thread-safe

---

## 🚀 Installation

Add the project reference or include the NuGet package (if published):

```bash
dotnet add package InMemoryLogger

