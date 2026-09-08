public class ConsumedFood
{
    public int Id { get; set; }
    public DateTime ConsumedAt { get; set; }
    public int FoodId { get; set; }

    public double Weight { get; set; }

    public string Name { get; set; } = "";
    public double Calories { get; set; }
    public double Proteins { get; set; }
}