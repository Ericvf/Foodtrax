using Dapper;

public class ConsumedFoodRepository
{
    private readonly SqlLiteService sqlite;

    public ConsumedFoodRepository(SqlLiteService sqlite)
    {
        this.sqlite = sqlite;
    }

    public async Task<int> AddCustomAsync(ConsumedFood food)
    {
        using var connection = sqlite.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            """
        INSERT INTO ConsumedFood (
            ConsumedAt,
            Name,
            Unit,
            Amount,
            Calories,
            Proteins,
            UserId
        )
        VALUES (
            @ConsumedAt,
            @Name,
            @Unit,
            @Amount,
            @Calories,
            @Proteins,
            @UserId
        );

        SELECT last_insert_rowid();
        """,
            new
            {
                ConsumedAt = food.ConsumedAt.ToUniversalTime().ToString("O"),
                food.Name,
                food.Unit,
                food.Amount,
                food.Calories,
                food.Proteins,
                food.UserId
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

        using var connection = sqlite.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
            """
        INSERT INTO ConsumedFood (
            ConsumedAt,
            Name,
            Unit,
            Amount,
            Calories,
            Proteins,
            UserId
        )
        VALUES (
            @ConsumedAt,
            @Name,
            @Unit,
            @Amount,
            @Calories,
            @Proteins,
            @UserId
        );

        SELECT last_insert_rowid();
        """,
            new
            {
                ConsumedAt = consumedAt.ToUniversalTime().ToString("O"),
                food.Name,
                food.Unit,
                Amount = consumedAmount,
                Calories = calories,
                Proteins = proteins,
                food.UserId
            });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = sqlite.CreateConnection();

        var affected = await connection.ExecuteAsync(
            """
            DELETE FROM ConsumedFood
            WHERE Id = @Id
            """,
            new { Id = id });

        return affected > 0;
    }

    public async Task<IEnumerable<ConsumedFood>> GetByDateAsync(DateTime date, string userId)
    {
        using var connection = sqlite.CreateConnection();

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
            Proteins,
            UserId
        FROM ConsumedFood
        WHERE ConsumedAt >= @Start
          AND ConsumedAt < @End
          AND UserId = @UserId
          
        ORDER BY ConsumedAt
        """,
            new
            {
                Start = start.ToString("O"),
                End = end.ToString("O"),
                UserId = userId
            });
    }
}