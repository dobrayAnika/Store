namespace StoreApp.Models
{
    public interface IDeliveryService
    {
        void DeliverProduct(Product product, Store store);
    }
}