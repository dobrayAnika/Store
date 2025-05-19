using System;

namespace StoreApp.Models
{
    public class DeliveryService : IDeliveryService
    {
        public void DeliverProduct(Product product, Store store)
        {
            Console.WriteLine($"Доставка: добавлен товар {product.Name} в магазин {store.Name}");
            store.DeliverProduct(product);
        }
    }
}