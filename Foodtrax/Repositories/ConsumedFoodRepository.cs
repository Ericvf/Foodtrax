using Dapper;
using Foodtrax.Models;
using Foodtrax.Services;

public class ConsumedFoodRepository
{
    private readonly SqlLiteService _sqlite;

    public ConsumedFoodRepository(SqlLiteService sqlite)
    {
        _sqlite = sqlite;
    }

    public async Task<int> AddCustomAsync(ConsumedFood food)
    {
        using var connection = _sqlite.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            """
        INSERT INTO ConsumedFood (
            ConsumedAt,
            Name,
            Unit,
            Amount,
            Calories,
            Proteins
        )
        VALUES (
            @ConsumedAt,
            @Name,
            @Unit,
            @Amount,
            @Calories,
            @Proteins
        );

        SELECT last_insert_rowid();
        """,
            new
            {
                ConsumedAt = food.ConsumedAt.ToString("O"),
                food.Name,
                food.Unit,
                food.Amount,
                food.Calories,
                food.Proteins
            });
    }

    public async Task<int> AddAsync(
        Food food,
        double consumedAmount,
        DateTime consumedAt)
    {
        var multiplier = consumedAmount / food.Amount;

        var calories = food.Calories * multiplier;
        var proteins = food.Proteins * multiplier;

        using var connection = _sqlite.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            """
        INSERT INTO ConsumedFood (
            ConsumedAt,
            Name,
            Unit,
            Amount,
            Calories,
            Proteins
        )
        VALUES (
            @ConsumedAt,
            @Name,
            @Unit,
            @Amount,
            @Calories,
            @Proteins
        );

        SELECT last_insert_rowid();
        """,
            new
            {
                ConsumedAt = consumedAt.ToString("O"),
                food.Name,
                food.Unit,
                Amount = consumedAmount,
                Calories = calories,
                Proteins = proteins
            });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _sqlite.CreateConnection();

        var affected = await connection.ExecuteAsync(
            """
            DELETE FROM ConsumedFood
            WHERE Id = @Id
            """,
            new { Id = id });

        return affected > 0;
    }

    public async Task<IEnumerable<ConsumedFood>> GetByDateAsync(DateTime date)
    {
        using var connection = _sqlite.CreateConnection();

        var start = date.Date;
        var end = start.AddDays(1);

        return await connection.QueryAsync<ConsumedFood>(
            """
        SELECT
            Id,
            ConsumedAt,
            Name,
            Unit,
            Amount,
            Calories,
            Proteins
        FROM ConsumedFood
        WHERE ConsumedAt >= @Start
          AND ConsumedAt < @End
        ORDER BY ConsumedAt
        """,
            new
            {
                Start = start.ToString("O"),
                End = end.ToString("O")
            });
    }
}