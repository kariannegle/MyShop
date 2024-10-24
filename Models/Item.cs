using System; //provides a way to use classes in the System namespace

namespace MyShop.Models
{
    public class Item // class is a blueprint for creating objects
    {
        // Properties
        // variables must begin with upper case to follow C# convention
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty; // mandatory to have a value
        public decimal Price { get; set; }
        public string? Description { get; set; } // ? means it can be null
        public string? ImageUrl { get; set; } 
        // Navigation property
        public virtual List<OrderItem>? OrderItems { get; set; }
    }
}