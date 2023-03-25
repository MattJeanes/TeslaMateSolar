using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using PubSub;
using TeslaMateSolar.Data;
using TeslaMateSolar.Providers.Solar.Interfaces;
using TeslaMateSolar.Providers.Tesla.Interfaces;

namespace TeslaMateSolar;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly AppSettings _appSettings;
    private readonly ITeslaProvider _teslaProvider;
    private readonly ISolarProvider _solarProvider;
    private readonly Hub _hub;
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _mqttClientOptions;
    private bool _mqttConnected;

    public Worker(
        ILogger<Worker> logger,
        IOptions<AppSettings> appSettings,
        ITeslaProvider teslaProvider,
        ISolarProvider solarProvider,
        Hub hub)
    {
        _logger = logger;
        _appSettings = appSettings.Value;
        _teslaProvider = teslaProvider;
        _solarProvider = solarProvider;
        _hub = hub;

        if (_teslaProvider is IMqttTeslaProvider || _solarProvider is IMqttSolarProvider)
        {
            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();
            _mqttClientOptions = new MqttClientOptionsBuilder().WithConnectionUri(_appSettings.MqttConnectionUri).Build();
            _mqttClient.ApplicationMessageReceivedAsync += HandleMessageAsync;
            _mqttConnected = false;
        }

        _logger.LogInformation("Using Solar Provider {SolarProvider}", _solarProvider.GetType().Name);

        _hub.Subscribe<TeslaState>(HandleTeslaStateUpdateAsync);
        _hub.Subscribe<SolarState>(HandleSolarStateUpdateAsync);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_mqttClient != null && !await _mqttClient.TryPingAsync())
                {
                    if (_mqttConnected)
                    {
                        _logger.LogWarning("Lost connection with MQTT server, attempting to reconnect");
                        _mqttConnected = false;
                    }
                    await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
                    _logger.LogInformation("Established connection with MQTT server");
                    _mqttConnected = true;

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
        if (_teslaProvider is IMqttTeslaProvider mqttTeslaProvider)
        {
            foreach (var topicName in mqttTeslaProvider.MqttTopics)
            {
                _logger.LogInformation("Subscribing to topic {TopicName} for {TeslaProvider}", topicName, _teslaProvider.GetType().Name);
                builder.WithTopicFilter(topicName);
            }
        }
        if (_solarProvider is IMqttSolarProvider mqttSolarProvider)
        {
            foreach (var topicName in mqttSolarProvider.MqttTopics)
            {
                _logger.LogInformation("Subscribing to topic {TopicName} for {SolarProvider}", topicName, _solarProvider.GetType().Name);
                builder.WithTopicFilter(topicName);
            }
        }
        await _mqttClient.SubscribeAsync(builder.Build());
    }

    private async Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        if (_teslaProvider is IMqttTeslaProvider mqttTeslaProvider && mqttTeslaProvider.MqttTopics.Contains(e.ApplicationMessage.Topic))
        {
            await mqttTeslaProvider.HandleMessageAsync(e);
        }
        else if (_solarProvider is IMqttSolarProvider mqttSolarProvider && mqttSolarProvider.MqttTopics.Contains(e.ApplicationMessage.Topic))
        {
            await mqttSolarProvider.HandleMessageAsync(e);
        }
        else
        {
            _logger.LogWarning("Unhandled MQTT message on topic {TopicName}", e.ApplicationMessage.Topic);
        }
    }

    private Task HandleSolarStateUpdateAsync(SolarState state)
    {
        _logger.LogInformation("Received solar state update as of {Timestamp}: Load: {Load}W, Grid In: {GridIn}W, Grid Out: {GridOut}W, Solar: {Solar}W", state.Timestamp, state.LoadWatts, state.GridInWatts, state.GridOutWatts, state.SolarWatts);
        return Task.CompletedTask;
    }

    private Task HandleTeslaStateUpdateAsync(TeslaState state)
    {
        _logger.LogInformation("Received tesla state update as of {Timestamp}: Status: {Status}", state.Timestamp, state.State);
        return Task.CompletedTask;
    }
}
