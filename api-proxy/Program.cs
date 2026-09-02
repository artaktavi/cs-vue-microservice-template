using Shared.Observability;

namespace ApiProxy;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddSharedObservability("api-proxy");
        builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

        var app = builder.Build();
        app.UseSharedHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseSharedObservability();
        app.MapReverseProxy();
        app.MapFallbackToFile("index.html");
        app.Run();
    }
}
