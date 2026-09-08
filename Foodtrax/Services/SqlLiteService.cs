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
                    Calories REAL NOT NULL,
                    Proteins REAL NOT NULL,
                    Weight REAL NOT NULL
                );

                CREATE TABLE IF NOT EXISTS ConsumedFood (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ConsumedAt TEXT NOT NULL,
                    FoodId INTEGER NOT NULL,
                    Weight REAL NOT NULL,
                    FOREIGN KEY (FoodId) REFERENCES Food(Id)
                );

                CREATE INDEX IF NOT EXISTS IX_ConsumedFood_ConsumedAt
                    ON ConsumedFood (ConsumedAt);

                CREATE INDEX IF NOT EXISTS IX_ConsumedFood_FoodId
                    ON ConsumedFood (FoodId);
            """;

            return command.ExecuteNonQueryAsync();
        }
    }
}
