using System;

namespace StoreApp.Models
{
    public class ProductEventArgs : EventArgs
    {
        public Product? Product { get; }
        public Customer Customer { get; }

        public ProductEventArgs(Product? product, Customer customer)
        {
            Product = product;
            Customer = customer;
        }
    }
}