using Dapper;
using Foodtrax.Services;

public class ConsumedFoodRepository
{
    private readonly SqlLiteService _sqlite;

    public ConsumedFoodRepository(SqlLiteService sqlite)
    {
        _sqlite = sqlite;
    }

    public async Task<int> AddAsync(
        int foodId,
        double weight,
        DateTime consumedAt)
    {
        using var connection = _sqlite.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            """
            INSERT INTO ConsumedFood (
                ConsumedAt,
                FoodId,
                Weight
            )
            VALUES (
                @ConsumedAt,
                @FoodId,
                @Weight
            );

            SELECT last_insert_rowid();
            """,
            new
            {
                ConsumedAt = consumedAt.ToString("O"),
                FoodId = foodId,
                Weight = weight
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
            cf.Id,
            cf.ConsumedAt,
            cf.FoodId,
            cf.Weight,
            f.Name,
            f.Calories * cf.Weight / 100.0 AS Calories,
            f.Proteins * cf.Weight / 100.0 AS Proteins
        FROM ConsumedFood cf
        INNER JOIN Food f ON f.Id = cf.FoodId
        WHERE cf.ConsumedAt >= @Start
          AND cf.ConsumedAt < @End
        ORDER BY cf.ConsumedAt
        """,
            new
            {
                Start = start.ToString("O"),
                End = end.ToString("O")
            });
    }
}