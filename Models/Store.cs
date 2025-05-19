using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;


namespace StoreApp.Models
{
    public class Store
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public ObservableCollection<Product> Products { get; } = new();

        public event EventHandler<ProductEventArgs>? ProductSold;
        public event EventHandler? OutOfStock;
        public event EventHandler<ProductEventArgs>? ProductDelivery;

        public Store(string name, double x, double y)
        {
            Name = name;
            X = x;
            Y = y;
            ProductSold = null!;
            OutOfStock = null!;
            ProductDelivery = null!;
        }

        public void TrySellProduct(Customer customer)
        {
            if (!Products.Any())
            {
                OutOfStock?.Invoke(this, EventArgs.Empty);
                return;
            }

            var random = new Random();
            var product = Products[random.Next(Products.Count)];

            if (product.Quantity > 0)
            {
                product.Quantity--;
                ProductSold?.Invoke(this, new ProductEventArgs(product, customer));

                if (product.Quantity == 0)
                {
                    OutOfStock?.Invoke(this, EventArgs.Empty);
                    StartDelivery(product);
                }
            }
        }

        private void StartDelivery(Product depletedProduct)
        {
            var delivery = new DeliveryService();
            var newProduct = new Product(depletedProduct.Name, depletedProduct.Price, 5); // новая партия
            ProductDelivery?.Invoke(this, new ProductEventArgs(depletedProduct, null));

            Task.Run(async () =>
            {
                await Task.Delay(2000); // "в пути"
                delivery.DeliverProduct(newProduct, this);
            });
        }

        public void DeliverProduct(Product product)
        {
            Products.Add(product);
            ProductDelivery?.Invoke(this, new ProductEventArgs(product, null));
        }
    }
}
