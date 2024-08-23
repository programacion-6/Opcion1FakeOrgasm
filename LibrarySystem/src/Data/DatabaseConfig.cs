using Dapper;
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
        var password = Environment.GetEnvironmentVariable("DB_PASS");

        _connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={dbName}";
    }

    public NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public static async Task CreateDatabaseSchema(NpgsqlConnection connection)
    {
        var createBooksTable = @"
            CREATE TABLE IF NOT EXISTS Books (
                Id UUID PRIMARY KEY,
                Title VARCHAR(255) NOT NULL,
                Author VARCHAR(255) NOT NULL,
                ISBN VARCHAR(13) NOT NULL,
                Genre VARCHAR(100),
                PublicationYear INT
            );";
        
        var createPatronsTable = @"
            CREATE TABLE IF NOT EXISTS Patrons (
                Id UUID PRIMARY KEY,
                Name VARCHAR(255) NOT NULL,
                MembershipNumber INT NOT NULL,
                ContactDetails TEXT NOT NULL
            );";

        var createLoansTable = @"
            CREATE TABLE IF NOT EXISTS Loans (
                Id UUID PRIMARY KEY,
                BookId UUID NOT NULL,
                PatronId UUID NOT NULL,
                LoanDate TIMESTAMP NOT NULL,
                ReturnDate TIMESTAMP NOT NULL,
                WasReturn BOOLEAN NOT NULL,
                FOREIGN KEY (BookId) REFERENCES Books(Id),
                FOREIGN KEY (PatronId) REFERENCES Patrons(Id)
            );";

        var createFinesTable = @"
            CREATE TABLE IF NOT EXISTS Fines (
                Id UUID PRIMARY KEY,
                LoanId UUID NOT NULL,
                FineAmount DOUBLE PRECISION NOT NULL,
                WasPayed BOOLEAN NOT NULL,
                FOREIGN KEY (LoanId) REFERENCES Loans(Id)
            );";

        await connection.ExecuteAsync(createBooksTable);
        await connection.ExecuteAsync(createPatronsTable);
        await connection.ExecuteAsync(createLoansTable);
        await connection.ExecuteAsync(createFinesTable);
    }

}