using MQTTnet.Client;

namespace TeslaMateSolar.Providers.Tesla.Interfaces;

public interface IMqttTeslaProvider : ITeslaProvider
{
    IEnumerable<string> MqttTopics { get; }
    Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e);
}
