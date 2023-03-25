using Microsoft.Extensions.Options;
using MQTTnet.Client;
using PubSub;
using System.Text.Json;
using TeslaMateSolar.Data;
using TeslaMateSolar.Data.Options;
using TeslaMateSolar.Data.Solar;
using TeslaMateSolar.Providers.Solar.Interfaces;

namespace TeslaMateSolar.Providers.Solar;

public class GrottProvider : IRedisProvider
{
    public IEnumerable<string> RedisTopics => new string[] { _options.TopicName };

    private readonly ILogger<GrottProvider> _logger;
    private readonly GrottOptions _options;
    private readonly Hub _hub;

    public GrottProvider(ILogger<GrottProvider> logger, IOptions<GrottOptions> options, Hub hub)
    {
        _logger = logger;
        _options = options.Value;
        _hub = hub;
    }

    public async Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        var message = JsonSerializer.Deserialize(e.ApplicationMessage.Payload, GrottContext.Default.GrottState);

        if (message.Buffered == "yes")
        {
            _logger.LogDebug("Ignoring buffered Grott message");
        }

        var state = new SolarState
        {
            Timestamp = message.Time,
            GridInWatts = message.Values.Pactouserr / 10,
            GridOutWatts = message.Values.Pactogridr / 10,
            SolarWatts = message.Values.Pvpowerin / 10,
            LoadWatts = message.Values.Plocaloadr / 10
        };

        await _hub.PublishAsync(state);
    }
}
