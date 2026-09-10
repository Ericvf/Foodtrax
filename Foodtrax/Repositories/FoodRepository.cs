using Dapper;
using Foodtrax.Models;
using Foodtrax.Services;

public class FoodRepository(SqlLiteService sqlite)
{
    private readonly SqlLiteService _sqlite = sqlite;

    public async Task<IEnumerable<Food>> GetAllAsync(string userId)
    {
        using var connection = _sqlite.CreateConnection();

        return await connection.QueryAsync<Food>(
            """
            SELECT
                Id,
                Name,
                Calories,
                Proteins,
                Amount,
                Unit
            FROM Food
            WHERE UserId = @UserId
            ORDER BY Name
            """, new
            {
                UserId = userId
            });
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
                Unit,
                Amount
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
                Unit,   
                Amount,
                UserId
            )
            VALUES (
                @Name,
                @Calories,
                @Proteins,
                @Unit,
                @Amount,
                @UserId
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