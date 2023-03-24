using TeslaMateSolar.Data;

namespace TeslaMateSolar;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.Configure<AppSettings>(builder.Configuration);
        builder.Services.AddHostedService<Worker>();

        var app = builder.Build();
        app.MapControllers();
        await app.RunAsync();
    }
}
