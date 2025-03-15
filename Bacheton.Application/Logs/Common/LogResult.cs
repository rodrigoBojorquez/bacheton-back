namespace Bacheton.Application.Logs.Common;

public record LogResult(string Id, string TraceId, string Message, string Level, string Timestamp);