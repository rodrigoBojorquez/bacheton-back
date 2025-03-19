namespace Bacheton.Application.User.Common;

public record TopUserResult(Guid Id, string Name, string Email, int ReportsCount);