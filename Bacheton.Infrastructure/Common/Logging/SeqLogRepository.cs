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

        return result.Select(e => new LogResult(Id: e.Id, TraceId: e.TraceId,
            Message: e.MessageTemplateTokens.Select(m => m.Text).ToList().First(),
            Level: e.Level, Timestamp: e.Timestamp)).ToList();
    }
}