using System.ComponentModel.DataAnnotations;

public class Food
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 200 characters")]
    public string Name { get; set; } = "";

    public string Unit { get; set; } = "g";

    public double Amount { get; set; } = 100;

    public double Calories { get; set; }

    public double Proteins { get; set; }

    public string UserId { get; set; } = "";
}
