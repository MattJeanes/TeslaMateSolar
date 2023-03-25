using TeslaMateSolar.Data.Solar;

namespace TeslaMateSolar.Providers.Solar.Interfaces;

public interface IRestApiSolarProvider : ISolarProvider
{
    public Task UpdateState(RestApiState restState);
}
