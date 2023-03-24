using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;
using TeslaMateSolar.Data;
using TeslaMateSolar.Data.Energy;

namespace TeslaMateSolar;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly AppSettings _appSettings;
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _mqttClientOptions;

    public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings)
    {
        _logger = logger;
        _appSettings = appSettings.Value;
        var mqttFactory = new MqttFactory();
        _mqttClient = mqttFactory.CreateMqttClient();
        _mqttClientOptions = new MqttClientOptionsBuilder().WithConnectionUri(_appSettings.MqttConnectionUri).Build();
        _mqttClient.ApplicationMessageReceivedAsync += HandleMessageAsync;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!await _mqttClient.TryPingAsync())
                {
                    await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
                    _logger.LogInformation("Established connection with MQTT server");

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
        builder.WithTopicFilter(f => f.WithTopic("energy/growatt"));
        await _mqttClient.SubscribeAsync(builder.Build());
    }

    private Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        if (e.ApplicationMessage.Topic == "energy/growatt")
        {
            var growatt = JsonSerializer.Deserialize(e.ApplicationMessage.Payload, GrowattContext.Default.Growatt);
            _logger.LogInformation("Battery SOC {SOC}", growatt.Values.SOC);
        }
        return Task.CompletedTask;
    }
}
