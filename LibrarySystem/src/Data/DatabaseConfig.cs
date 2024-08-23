using System.Data;
using Npgsql;

namespace LibrarySystem;

public class DatabaseConfig
{
    private readonly string _connectionString;

    public DatabaseConfig()
    {
        DotNetEnv.Env.Load();

        var host = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        _connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={dbName}";
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}