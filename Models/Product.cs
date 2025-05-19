using System;

namespace StoreApp.Models
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product(string name, decimal price, int quantity = 1)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }

}