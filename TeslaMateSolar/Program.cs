namespace TeslaMateSolar;

public class Program {
    public static async Task Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.MapControllers();
        await app.RunAsync();
    }
}
