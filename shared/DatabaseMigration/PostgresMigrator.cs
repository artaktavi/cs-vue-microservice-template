using Microsoft.Extensions.Logging;
using Npgsql;

namespace Shared.DatabaseMigration;

public sealed class PostgresMigrator(ILogger<PostgresMigrator> logger)
{
    public async Task MigrateAsync(
        string connectionString,
        string migrationsPath,
        CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(migrationsPath))
        {
            throw new DirectoryNotFoundException($"Migration directory was not found: {migrationsPath}");
        }

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using (var historyCommand = connection.CreateCommand())
        {
            historyCommand.CommandText = """
                CREATE TABLE IF NOT EXISTS __schema_migrations (
                    script_name text PRIMARY KEY,
                    applied_at timestamptz NOT NULL DEFAULT now()
                );
                """;
            await historyCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        foreach (var scriptPath in Directory.EnumerateFiles(migrationsPath, "*.sql").OrderBy(path => path))
        {
            var scriptName = Path.GetFileName(scriptPath);
            await using var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "SELECT EXISTS (SELECT 1 FROM __schema_migrations WHERE script_name = @scriptName);";
            checkCommand.Parameters.AddWithValue("scriptName", scriptName);
            if (await checkCommand.ExecuteScalarAsync(cancellationToken) is true)
            {
                continue;
            }

            await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
            await using var migrationCommand = connection.CreateCommand();
            migrationCommand.Transaction = transaction;
            migrationCommand.CommandText = await File.ReadAllTextAsync(scriptPath, cancellationToken);
            await migrationCommand.ExecuteNonQueryAsync(cancellationToken);

            await using var recordCommand = connection.CreateCommand();
            recordCommand.Transaction = transaction;
            recordCommand.CommandText = "INSERT INTO __schema_migrations (script_name) VALUES (@scriptName);";
            recordCommand.Parameters.AddWithValue("scriptName", scriptName);
            await recordCommand.ExecuteNonQueryAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        logger.LogInformation("Database migrations completed from {MigrationsPath}", migrationsPath);
    }
}
