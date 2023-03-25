using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using PubSub;
using TeslaMateSolar.Data;
using TeslaMateSolar.Providers.Solar.Interfaces;

namespace TeslaMateSolar;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly AppSettings _appSettings;
    private readonly ISolarProvider _solarProvider;
    private readonly Hub _hub;
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _mqttClientOptions;
    private bool _connected;

    public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings, ISolarProvider solarProvider, Hub hub)
    {
        _logger = logger;
        _appSettings = appSettings.Value;
        _solarProvider = solarProvider;
        _hub = hub;

        var mqttFactory = new MqttFactory();
        _mqttClient = mqttFactory.CreateMqttClient();
        _mqttClientOptions = new MqttClientOptionsBuilder().WithConnectionUri(_appSettings.MqttConnectionUri).Build();
        _mqttClient.ApplicationMessageReceivedAsync += HandleMessageAsync;
        _connected = false;

        _logger.LogInformation("Using Solar Provider {SolarProvider}", _solarProvider.GetType().Name);

        _hub.Subscribe<SolarState>(HandleStateUpdateAsync);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!await _mqttClient.TryPingAsync())
                {
                    if (_connected)
                    {
                        _logger.LogWarning("Lost connection with MQTT server, attempting to reconnect");
                        _connected = false;
                    }
                    await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
                    _logger.LogInformation("Established connection with MQTT server");
                    _connected = true;

                    await ConnectSubscriptions();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to MQTT server, retrying in 30 seconds");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }
    }

    private async Task ConnectSubscriptions()
    {
        var builder = new MqttClientSubscribeOptionsBuilder();
        builder.WithTopicFilter(f =>
        {
            f.WithTopic($"teslamate/cars/{_appSettings.CarId}/state");
        });
        if (_solarProvider is IRedisProvider redisProvider)
        {
            builder.WithTopicFilter(f =>
            {
                foreach (var topicName in redisProvider.RedisTopics)
                {
                    _logger.LogInformation("Subscribing to topic {TopicName} for {SolarProvider}", topicName, _solarProvider.GetType().Name);
                    f.WithTopic(topicName);
                }
            });
        }
        await _mqttClient.SubscribeAsync(builder.Build());
    }

    private async Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        if (_solarProvider is IRedisProvider redisProvider && redisProvider.RedisTopics.Contains(e.ApplicationMessage.Topic))
        {
            await redisProvider.HandleMessageAsync(e);
        }
        else
        {
            _logger.LogWarning("Unhandled Redis message on topic {TopicName}", e.ApplicationMessage.Topic);
        }
    }

    private Task HandleStateUpdateAsync(SolarState state)
    {
        _logger.LogInformation("Received solar state update: Load: {Load}W, Grid In: {GridIn}W, Grid Out: {GridOut}W, Solar: {Solar}W", state.LoadWatts, state.GridInWatts, state.GridOutWatts, state.SolarWatts);
        return Task.CompletedTask;
    }
}
