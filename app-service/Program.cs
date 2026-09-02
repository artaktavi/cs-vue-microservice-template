using AppService.Contracts;
using Shared.DatabaseMigration;
using Shared.Observability;

namespace AppService;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddSharedObservability("app-service");
        builder.Services.AddSingleton<PostgresMigrator>();

        var connectionString = builder.Configuration.GetConnectionString("AppDb")
            ?? throw new InvalidOperationException("Connection string 'AppDb' is required.");
        builder.Services.AddHealthChecks().AddNpgSql(connectionString, name: "postgres");

        var app = builder.Build();
        var migrationsPath = builder.Configuration["Migrations:Path"];
        if (!string.IsNullOrWhiteSpace(migrationsPath))
        {
            await app.Services.GetRequiredService<PostgresMigrator>()
                .MigrateAsync(connectionString, migrationsPath, app.Lifetime.ApplicationStopping);
        }

        app.UseSharedObservability();
        app.MapGet("/status", () => Results.Ok(new ServiceStatusResponse("app-service", "ready")))
            .WithName("GetServiceStatus");
        await app.RunAsync();
    }
}
