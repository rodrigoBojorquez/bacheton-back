using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Logs.Common;
using Microsoft.Extensions.Configuration;
using Seq.Api;

namespace Bacheton.Infrastructure.Common.Logging;

public class SeqLogRepository : ILogRepository
{
    private readonly SeqConnection _seqConnection;

    public SeqLogRepository(IConfiguration configuration)
    {
        var seqUrl = configuration.GetValue<string>("Seq:Url");
        var apiKey = configuration.GetValue<string>("Seq:ApiKey");

        _seqConnection = new SeqConnection(seqUrl, apiKey);
    }

    public async Task<List<LogResult>> ListAllAsync()
    {
        var result = await _seqConnection.Events.ListAsync(count: 1000);

        var formated = result.Select(e =>
        {
            var requestPath = e.Properties?.FirstOrDefault(p => p.Name == "RequestPath")?.Value?.ToString();
            var status = e.Properties?.FirstOrDefault(p => p.Name == "StatusCode")?.Value?.ToString();
            var method = e.Properties?.FirstOrDefault(p => p.Name == "RequestMethod")?.Value?.ToString();

            return new LogResult(
                Id: e.Id,
                Timestamp: e.Timestamp,
                Endpoint: requestPath,
                Status: status,
                TraceId: e.TraceId,
                Duration: e.Properties?.FirstOrDefault(p => p.Name == "Elapsed")?.Value?.ToString(),
                UserId: e.Properties?.FirstOrDefault(p => p.Name == "UserId")?.Value?.ToString(),
                Level: e.Level,
                Method: method
            );
        }).ToList();

        return formated;
    }
}