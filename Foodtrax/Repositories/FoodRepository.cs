using Dapper;
using Foodtrax.Models;
using Foodtrax.Services;

public class FoodRepository
{
    private readonly SqlLiteService _sqlite;

    public FoodRepository(SqlLiteService sqlite)
    {
        _sqlite = sqlite;
    }

    public async Task<IEnumerable<Food>> GetAllAsync()
    {
        using var connection = _sqlite.CreateConnection();

        return await connection.QueryAsync<Food>(
            """
            SELECT
                Id,
                Name,
                Calories,
                Proteins,
                Weight
            FROM Food
            ORDER BY Name
            """);
    }

    public async Task<Food?> GetByIdAsync(int id)
    {
        using var connection = _sqlite.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Food>(
            """
            SELECT
                Id,
                Name,
                Calories,
                Proteins,
                Weight
            FROM Food
            WHERE Id = @Id
            """,
            new { Id = id });
    }

    public async Task<int> CreateAsync(Food food)
    {
        using var connection = _sqlite.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            """
            INSERT INTO Food (
                Name,
                Calories,
                Proteins,
                Weight
            )
            VALUES (
                @Name,
                @Calories,
                @Proteins,
                @Weight
            );

            SELECT last_insert_rowid();
            """,
            food);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _sqlite.CreateConnection();

        var affected = await connection.ExecuteAsync(
            "DELETE FROM Food WHERE Id = @Id",
            new { Id = id });

        return affected > 0;
    }
}