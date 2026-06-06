using System.Text.Json;
using SalonApp.Models;

namespace SalonApp.Services;

public class FileStorageService
{
    private readonly string _dataPath;
    private readonly string _productsFile;
    private readonly string _salesFile;

    public FileStorageService(IWebHostEnvironment environment)
    {
        _dataPath = Path.Combine(environment.ContentRootPath, "Data");
        _productsFile = Path.Combine(_dataPath, "products.json");
        _salesFile = Path.Combine(_dataPath, "sales.json");

        // Ensure Data directory exists
        if (!Directory.Exists(_dataPath))
        {
            Directory.CreateDirectory(_dataPath);
        }

        // Initialize files if they don't exist
        if (!File.Exists(_productsFile))
        {
            SaveProducts(new List<Product>());
        }
        if (!File.Exists(_salesFile))
        {
            SaveSales(new List<Sale>());
        }
    }

    public List<Product> LoadProducts()
    {
        try
        {
            var json = File.ReadAllText(_productsFile);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }
        catch
        {
            return new List<Product>();
        }
    }

    public void SaveProducts(List<Product> products)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(products, options);
        File.WriteAllText(_productsFile, json);
    }

    public List<Sale> LoadSales()
    {
        try
        {
            var json = File.ReadAllText(_salesFile);
            return JsonSerializer.Deserialize<List<Sale>>(json) ?? new List<Sale>();
        }
        catch
        {
            return new List<Sale>();
        }
    }

    public void SaveSales(List<Sale> sales)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(sales, options);
        File.WriteAllText(_salesFile, json);
    }

    public void AddSale(Sale sale)
    {
        var sales = LoadSales();
        
        // Generate new ID
        if (sales.Count > 0)
        {
            sale.Id = sales.Max(s => s.Id) + 1;
        }
        else
        {
            sale.Id = 1;
        }
        
        sales.Add(sale);
        SaveSales(sales);
    }
}
