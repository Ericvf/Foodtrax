using Microsoft.Data.Sqlite;

namespace Foodtrax.Services
{
    public class SqlLiteService
    {
        private readonly string _connectionString;

        public SqlLiteService(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }


        public Task Initialize()
        {
            using var connection = CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = """
                PRAGMA foreign_keys = ON;

                CREATE TABLE IF NOT EXISTS Food (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Unit TEXT NOT NULL,
                    Amount REAL NOT NULL,
                    Calories REAL NOT NULL,
                    Proteins REAL NOT NULL,
                    UserId TEXT
                );

                CREATE TABLE IF NOT EXISTS ConsumedFood (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ConsumedAt TEXT NOT NULL,
                    Name TEXT NOT NULL,
                    Unit TEXT NOT NULL,
                    Amount REAL NOT NULL,
                    Calories REAL NOT NULL,
                    Proteins REAL NOT NULL,
                    UserId TEXT
                );

                CREATE INDEX IF NOT EXISTS IX_ConsumedFood_ConsumedAt
                    ON ConsumedFood (ConsumedAt);
            """;

            return command.ExecuteNonQueryAsync();
        }
    }
}
