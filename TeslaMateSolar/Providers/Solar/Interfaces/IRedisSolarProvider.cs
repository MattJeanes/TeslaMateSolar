using MQTTnet.Client;

namespace TeslaMateSolar.Providers.Solar.Interfaces;

public interface IMqttSolarProvider : ISolarProvider
{
    IEnumerable<string> MqttTopics { get; }
    Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e);
}
