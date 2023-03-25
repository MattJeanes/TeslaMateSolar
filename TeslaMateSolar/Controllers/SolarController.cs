using Microsoft.AspNetCore.Mvc;
using TeslaMateSolar.Data.Enums;
using TeslaMateSolar.Data.Solar;
using TeslaMateSolar.Providers.Solar.Interfaces;

namespace TeslaMateSolar.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolarController : ControllerBase
{
    private readonly ILogger<SolarController> _logger;
    private readonly ISolarProvider _solarProvider;

    public SolarController(ILogger<SolarController> logger, ISolarProvider solarProvider)
    {
        _logger = logger;
        _solarProvider = solarProvider;
    }

    [HttpPost]
    public async Task<ActionResult> UpdateState([FromBody] RestApiState state)
    {
        if (_solarProvider is IRestApiProvider restApiProvider)
        {
            await restApiProvider.UpdateState(state);
            return Accepted();
        }
        else
        {
            ModelState.AddModelError(nameof(SolarProvider), $"{nameof(SolarProvider)} is not set to {nameof(SolarProvider.RestApi)}");
            return BadRequest(ModelState);
        }
    }
}
