namespace SalonApp.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // "телефон" или "аксессуар"
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
