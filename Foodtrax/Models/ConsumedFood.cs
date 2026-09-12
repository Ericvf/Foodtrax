public class ConsumedFood
{
    public int Id { get; set; }

    public DateTime ConsumedAt { get; set; }

    public string Name { get; set; } = "";

    public string Unit { get; set; } = "g";

    public double Amount { get; set; }

    public double Calories { get; set; }

    public double Proteins { get; set; }

    public string UserId { get; set; } = "";
}
