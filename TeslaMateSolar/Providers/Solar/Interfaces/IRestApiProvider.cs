using TeslaMateSolar.Data.Solar;

namespace TeslaMateSolar.Providers.Solar.Interfaces;

public interface IRestApiProvider : ISolarProvider
{
    public Task UpdateState(RestApiState restState);
}
