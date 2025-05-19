using ReactiveUI;
using StoreApp.Models;
using StoreApp.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using System;

namespace StoreApp.ViewModels
{
    public class StoreViewModel : ViewModelBase
    {
        public ObservableCollection<Store> Stores { get; } = new();
        public ObservableCollection<MovableCustomerViewModel> Customers { get; } = new();
        public ObservableCollection<string> EventLog { get; } = new();

        public ObservableCollection<object> Models { get; } = new();

        public ICommand AddStoreCommand { get; }
        public ICommand AddCustomerCommand { get; }

        private readonly Timer _timer;

        public StoreViewModel()
        {
            AddStoreCommand = ReactiveCommand.Create(AddStore);
            AddCustomerCommand = ReactiveCommand.Create(AddCustomer);

            _timer = new Timer(100);
            _timer.Elapsed += (_, _) => MoveCustomers();
            _timer.Start();
        }

        private void AddStore()
        {
            if (Stores.Count > 0) return;
            var store = new Store($"Магазин 1", 100, 100);
            store.Products.Add(new Product("Яблоко", 10, 3));
            store.Products.Add(new Product("Хлеб", 20, 2));
            store.Products.Add(new Product("Молоко", 15, 4));
            store.Products.Add(new Product("Сыр", 30, 1));
            var storeVm = new StoreModelView(store);
            store.ProductSold += OnProductSold;
            store.OutOfStock += OnOutOfStock;
            store.ProductDelivery += OnProductDelivered;

            ReflectionHelper.PrintObjectInfo(store);

            // Обновление ObservableCollection через UI-поток
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                Stores.Add(store);
                Models.Add(storeVm);
            });
        }

        private void AddCustomer()
        {
            var customer = new Customer($"Покупатель {Customers.Count + 1}", 50, 300 + 30 * Customers.Count);
            var vm = new MovableCustomerViewModel(customer);

            ReflectionHelper.PrintObjectInfo(customer);

            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                Customers.Add(vm);
                Models.Add(vm);
            });

            // Покупка товара после добавления покупателя
            var store = Stores.FirstOrDefault();
            store?.TrySellProduct(customer);
        }



        private async void MoveCustomers()
        {
            foreach (var customer in Customers.ToList())
            {
                if (!Stores.Any()) continue;

                var nearestStore = Stores.OrderBy(s => Distance(customer.X, customer.Y, s.X, s.Y)).First();

                customer.MoveTowards(nearestStore.X, nearestStore.Y);

                if (customer.IsNear(nearestStore.X, nearestStore.Y))
                {
                    await customer.Customer.BuyAsync(nearestStore);

                    Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    {
                        Customers.Remove(customer);
                        if (Models.Contains(customer))
                        {
                            Models.Remove(customer);
                        }
                    });
                }
            }
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            double dx = x1 - x2;
            double dy = y1 - y2;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private void OnProductSold(object? sender, ProductEventArgs e)
        {
            if (sender == null || e == null || e.Product == null || e.Customer == null)
            {
                Console.WriteLine("Один из объектов равен null:");
                Console.WriteLine($"sender: {sender}");
                Console.WriteLine($"e: {e}");
                Console.WriteLine($"e.Product: {e?.Product}");
                Console.WriteLine($"e.Customer: {e?.Customer}");
                return;
            }

            if (EventLog == null)
            {
                Console.WriteLine("EventLog равен null");
                return;
            }

            EventLog.Insert(0, $"{e.Customer.Name} купил товар {e.Product.Name}");
        }

        private void OnOutOfStock(object? sender, EventArgs e)
        {
            EventLog.Insert(0, "Товар закончился! Отправлена доставка...");
        }

        private void OnProductDelivered(object? sender, ProductEventArgs e)
        {
            if (e.Product == null) return;
            EventLog.Insert(0, $"Доставка товара \"{e.Product.Name}\" пришла!");
        }
    }
}