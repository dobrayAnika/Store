using ReactiveUI;
using StoreApp.Models;
using System;

namespace StoreApp.ViewModels
{
    public class MovableCustomerViewModel : ViewModelBase
    {
        private double _x;
        private double _y;
        public Customer Customer { get; }

        public double X
        {
            get => _x;
            set => this.RaiseAndSetIfChanged(ref _x, value);
        }

        public double Y
        {
            get => _y;
            set => this.RaiseAndSetIfChanged(ref _y, value);
        }

        public string Name => Customer.Name;

        public MovableCustomerViewModel(Customer customer)
        {
            Customer = customer;
            X = customer.X;
            Y = customer.Y;
        }

        public void MoveTowards(double targetX, double targetY)
        {
            const double step = 5.0;
            double dx = targetX - X;
            double dy = targetY - Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            if (dist < step) return;

            X += step * dx / dist;
            Y += step * dy / dist;
        }

        public bool IsNear(double targetX, double targetY)
        {
            double dx = targetX - X;
            double dy = targetY - Y;
            return Math.Sqrt(dx * dx + dy * dy) < 10.0;
        }
    }
}
