using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using MyShop.DAL;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Controllers;

public class CustomerController : Controller 
{
    private readonly ItemDbContext _itemDbContext; // Private readonly field to hold the ItemDbContext instance

    public CustomerController(ItemDbContext itemDbContext) // Constructor to inject the ItemDbContext instance
    {
        _itemDbContext = itemDbContext; // Assign the injected ItemDbContext instance to the private field
    }

    public async Task<IActionResult> Table() // Action method to display the customers in a table
    {
        List<Customer> customers = await _itemDbContext.Customers.ToListAsync();
        return View(customers);
    }
}