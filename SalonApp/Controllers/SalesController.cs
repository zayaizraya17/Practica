using Microsoft.AspNetCore.Mvc;
using SalonApp.Models;
using SalonApp.Services;

namespace SalonApp.Controllers;

public class SalesController : Controller
{
    private readonly FileStorageService _storage;

    public SalesController(FileStorageService storage)
    {
        _storage = storage;
    }

    public IActionResult Index()
    {
        var sales = _storage.LoadSales().OrderByDescending(s => s.Date).ToList();
        return View(sales);
    }

    public IActionResult Create()
    {
        var products = _storage.LoadProducts().Where(p => p.Stock > 0).ToList();
        ViewBag.Products = products;
        return View();
    }

    [HttpPost]
    public IActionResult Create(int productId, int quantity)
    {
        var products = _storage.LoadProducts();
        var product = products.FirstOrDefault(p => p.Id == productId);

        if (product == null)
        {
            ModelState.AddModelError("", "Товар не найден");
            ViewBag.Products = products.Where(p => p.Stock > 0).ToList();
            return View();
        }

        if (quantity <= 0)
        {
            ModelState.AddModelError("", "Количество должно быть больше 0");
            ViewBag.Products = products.Where(p => p.Stock > 0).ToList();
            return View();
        }

        if (product.Stock < quantity)
        {
            ModelState.AddModelError("", $"Недостаточно товара на складе. Доступно: {product.Stock}");
            ViewBag.Products = products.Where(p => p.Stock > 0).ToList();
            return View();
        }

        // Calculate total
        var total = product.Price * quantity;

        // Update stock
        product.Stock -= quantity;
        _storage.SaveProducts(products);

        // Create sale record
        var sale = new Sale
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = quantity,
            Total = total,
            Date = DateTime.Now
        };

        _storage.AddSale(sale);

        TempData["SuccessMessage"] = $"Продажа оформлена на {total:N0} ₽";
        return RedirectToAction(nameof(Index));
    }
}
