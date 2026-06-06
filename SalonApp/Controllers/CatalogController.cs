using Microsoft.AspNetCore.Mvc;
using SalonApp.Models;
using SalonApp.Services;

namespace SalonApp.Controllers;

public class CatalogController : Controller
{
    private readonly FileStorageService _storage;

    public CatalogController(FileStorageService storage)
    {
        _storage = storage;
    }

    public IActionResult Index()
    {
        var products = _storage.LoadProducts();
        return View(products);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            var products = _storage.LoadProducts();
            
            // Generate new ID
            if (products.Count > 0)
            {
                product.Id = products.Max(p => p.Id) + 1;
            }
            else
            {
                product.Id = 1;
            }
            
            products.Add(product);
            _storage.SaveProducts(products);
            
            return RedirectToAction(nameof(Index));
        }
        
        return View(product);
    }

    public IActionResult Edit(int id)
    {
        var products = _storage.LoadProducts();
        var product = products.FirstOrDefault(p => p.Id == id);
        
        if (product == null)
        {
            return NotFound();
        }
        
        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(int id, Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var products = _storage.LoadProducts();
            var existingProduct = products.FirstOrDefault(p => p.Id == id);
            
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Type = product.Type;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                
                _storage.SaveProducts(products);
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        return View(product);
    }

    public IActionResult Delete(int id)
    {
        var products = _storage.LoadProducts();
        var product = products.FirstOrDefault(p => p.Id == id);
        
        if (product == null)
        {
            return NotFound();
        }
        
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var products = _storage.LoadProducts();
        var product = products.FirstOrDefault(p => p.Id == id);
        
        if (product != null)
        {
            products.Remove(product);
            _storage.SaveProducts(products);
        }
        
        return RedirectToAction(nameof(Index));
    }
}
