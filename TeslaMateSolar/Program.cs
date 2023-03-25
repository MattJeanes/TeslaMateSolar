using PubSub;
using TeslaMateSolar.Data;
using TeslaMateSolar.Data.Enums;
using TeslaMateSolar.Data.Options;
using TeslaMateSolar.Providers.Solar;
using TeslaMateSolar.Providers.Solar.Interfaces;
using TeslaMateSolar.Providers.Tesla;
using TeslaMateSolar.Providers.Tesla.Interfaces;

namespace TeslaMateSolar;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;
        var config = builder.Configuration;

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddOptions<AppSettings>()
            .Bind(config)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddSingleton(Hub.Default);
        services.AddSingleton<ITeslaProvider, TeslaMateProvider>();
        services.AddOptions<TeslaMateOptions>()
            .BindConfiguration("TeslaMate")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var solarProvider = config.GetValue<SolarProvider?>("SolarProvider");

        switch (solarProvider)
        {
            case SolarProvider.Grott:
                services.AddOptions<GrottOptions>()
                    .BindConfiguration("Grott")
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
                services.AddSingleton<ISolarProvider, GrottSolarProvider>();
                break;
            case SolarProvider.RestApi:
                services.AddSingleton<ISolarProvider, RestApiSolarProvider>();
                break;
            case null:
                throw new ArgumentNullException(nameof(solarProvider));
            default:
                throw new ArgumentOutOfRangeException(nameof(solarProvider));
        }

        services.AddHostedService<Worker>();

        var app = builder.Build();
        app.MapControllers();
        await app.RunAsync();
    }
}
