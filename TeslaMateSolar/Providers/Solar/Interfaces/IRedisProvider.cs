using MQTTnet.Client;

namespace TeslaMateSolar.Providers.Solar.Interfaces;

public interface IRedisProvider : ISolarProvider
{
    IEnumerable<string> RedisTopics { get; }
    Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e);
}
