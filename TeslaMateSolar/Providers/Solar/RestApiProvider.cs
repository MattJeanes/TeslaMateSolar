using PubSub;
using TeslaMateSolar.Data;
using TeslaMateSolar.Data.Solar;
using TeslaMateSolar.Providers.Solar.Interfaces;

namespace TeslaMateSolar.Providers.Solar;

public class RestApiProvider : IRestApiProvider
{
    private readonly ILogger<RestApiProvider> _logger;
    private readonly Hub _hub;

    public RestApiProvider(ILogger<RestApiProvider> logger, Hub hub)
    {
        _logger = logger;
        _hub = hub;
    }

    public async Task UpdateState(RestApiState restState)
    {
        var state = new SolarState
        {
            Timestamp = restState.Timestamp ?? DateTimeOffset.UtcNow,
            GridInWatts = restState.GridInWatts.Value,
            GridOutWatts = restState.GridOutWatts.Value,
            SolarWatts = restState.SolarWatts.Value,
            LoadWatts = restState.LoadWatts.Value
        };
        await _hub.PublishAsync(state);
    }
}
