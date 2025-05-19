using StoreApp.Models;

namespace StoreApp.ViewModels
{
    public class CustomerViewModel
    {
        public Customer Customer { get; }
        public string Name => Customer.Name;

        public CustomerViewModel(Customer customer)
        {
            Customer = customer;
        }
    }
}