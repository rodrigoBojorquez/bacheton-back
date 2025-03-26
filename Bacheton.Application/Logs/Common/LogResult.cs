namespace Bacheton.Application.Logs.Common;

public record LogResult(
    string Id, 
    string? Timestamp,
    string? Endpoint, 
    string? Status,
    string? TraceId, 
    string? Duration,
    string? UserId,
    string Level,
    string? Method);