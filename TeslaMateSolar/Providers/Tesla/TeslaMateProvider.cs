using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using PubSub;
using TeslaMateSolar.Data;
using TeslaMateSolar.Data.Options;
using TeslaMateSolar.Providers.Tesla.Interfaces;

namespace TeslaMateSolar.Providers.Tesla;

public class TeslaMateProvider : IMqttTeslaProvider
{
    public IEnumerable<string> MqttTopics => Topics.Select(topic => $"teslamate/cars/{_options.CarId}/{topic}");

    private readonly ILogger<TeslaMateProvider> _logger;
    private readonly TeslaMateOptions _options;
    private readonly Hub _hub;
    private readonly TeslaState _state = new();
    private bool _ready;

    private readonly IEnumerable<string> Topics = new string[]
    {
        "state",
        "since"
    };

    private readonly HashSet<string> TopicsReceived = new HashSet<string>();

    public TeslaMateProvider(ILogger<TeslaMateProvider> logger, IOptions<TeslaMateOptions> options, Hub hub)
    {
        _logger = logger;
        _options = options.Value;
        _hub = hub;
    }

    public async Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        var value = e.ApplicationMessage.ConvertPayloadToString();
        var type = e.ApplicationMessage.Topic.Split("/").Last();
        switch (type)
        {
            case "state":
                _state.State = value;
                break;
            case "since":
                _state.Timestamp = DateTimeOffset.Parse(value);
                break;
            default:
                _logger.LogWarning("Unhandled TeslaMate message type {Type}", type);
                return;
        }

        if (!TopicsReceived.Contains(type))
        {
            TopicsReceived.Add(type);
            if (Topics.All(TopicsReceived.Contains))
            {
                _logger.LogInformation("Initial data loaded from TeslaMate");
                _ready = true;
            }
        }

        if (_ready)
        {
            await _hub.PublishAsync(_state);
        }
    }
}
