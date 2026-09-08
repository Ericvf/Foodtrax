namespace Foodtrax.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public double Calories { get; set; }
        public double Proteins { get; set; }

        public double Weight { get; set; } = 100;
    }
}
