namespace Bacheton.Application.Common.Results;

public record ListResult<T>(int Page, int PageSize, int TotalItems, IEnumerable<T> Items) where T : class;