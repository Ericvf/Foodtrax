public class FoodSelectedEventArgs
{
    public Food Food { get; set; } = null!;

    public double Amount { get; set; }
     
    public DateTime ConsumedAt { get; set; }
}