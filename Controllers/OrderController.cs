using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using Microsoft.EntityFrameworkCore;
using MyShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyShop.Controllers;

public class OrderController : Controller
{
    private readonly ItemDbContext _itemDbContext; // Private readonly field to hold the ItemDbContext instance

    public OrderController(ItemDbContext itemDbContext) // Constructor to inject the ItemDbContext instance
    {
        _itemDbContext = itemDbContext;
    }

    public async Task<IActionResult> Table() // Action method to display the orders in a table
    {
        List<Order> orders = await _itemDbContext.Orders.ToListAsync();
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> CreateOrderItem() // Action method to display the form to create an OrderItem
    {
        var items = await _itemDbContext.Items.ToListAsync(); // Retrieve all items from the database and convert them to a list
        var orders = await _itemDbContext.Orders.ToListAsync();
        var createOrderItemViewModel = new CreateOrderItemViewModel
        {
            OrderItem = new OrderItem(),

            ItemSelectList = items.Select(item => new SelectListItem // Wraps the info of ItemIds and Item Names into ItemSelectList
            {
                Value = item.ItemId.ToString(),
                Text = item.ItemId.ToString() + ": " + item.Name
            }).ToList(),

            OrderSelectList = orders.Select(order => new SelectListItem // wraps the information of OrderIds, OrderDates, and Customer Names into the OrderSelectList.
            {
                Value = order.OrderId.ToString(),
                Text = "Order" + order.OrderId.ToString() + ", Date: " + order.OrderDate + ", Customer: " + order.Customer.Name
            }).ToList(),
        };
        return View(createOrderItemViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderItem(OrderItem orderItem)
    {
        try
        {
            var newItem = _itemDbContext.Items.Find(orderItem.ItemId);
            var newOrder = _itemDbContext.Orders.Find(orderItem.OrderId);

            if (newItem == null || newOrder == null)
            {
                return BadRequest("Item or Order not found.");
            }

            var newOrderItem = new OrderItem
            {
                ItemId = orderItem.ItemId,
                Item = newItem,
                Quantity = orderItem.Quantity,
                OrderId = orderItem.OrderId,
                Order = newOrder,
            };
            newOrderItem.OrderItemPrice = orderItem.Quantity * newOrderItem.Item.Price;

            _itemDbContext.OrderItems.Add(newOrderItem);
            await _itemDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Table));
        }
        catch
        {
            return BadRequest("OrderItem creation failed.");
        }
    }
}