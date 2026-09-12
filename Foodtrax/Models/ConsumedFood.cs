using System.ComponentModel.DataAnnotations;

public class ConsumedFood
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Consumed at date is required")]
    public DateTime ConsumedAt { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 200 characters")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Unit is required")]
    public string Unit { get; set; } = "g";

    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public double Amount { get; set; }

    [Required(ErrorMessage = "Calories is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Calories must be 0 or greater")]
    public double Calories { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Proteins must be 0 or greater")]
    public double Proteins { get; set; }

    public string UserId { get; set; } = "";
}
