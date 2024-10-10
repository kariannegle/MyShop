// Purpose: Contains the ItemController class which is responsible for handling requests to the Item model.

// using directive is used to include the System namespace in the file
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyShop.Models;

namespace MyShop.Controllers; // namespace declaration

    // Creates a list of items in a mock fashion.
    // Sets a value in ViewBag to pass additional data to the View.
    // Returns a View that will render the list of items.

public class ItemController : Controller
{
    public IActionResult Table()
    {
        var items = new List<Item>(); // creates a new list of items
        var item1 = new Item(); // creates a new item object
        item1.ItemId = 1;
        item1.Name = "Pizza";
        item1.Price = 60;

        var item2 = new Item
        {
            ItemId = 2,
            Name = "Fried Chicken Leg",
            Price = 15
        };

        items.Add(item1);
        items.Add(item2);

        ViewBag.CurrentViewName = "List of Shop Items";
        return View(items);
    }
}