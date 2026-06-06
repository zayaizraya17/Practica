namespace SalonApp.Models;

public class Sale
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public DateTime Date { get; set; }
}
