using System;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class Customer
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public Customer(string name, double x, double y)
        {
            Name = name;
            X = x;
            Y = y;
        }

        public async Task BuyAsync(Store store)
        {
            await Task.Delay(1000);
            double chance = new Random().NextDouble();
            if (chance > 0.3)
            {
                store.TrySellProduct(this);
            }
            else
            {
                Console.WriteLine($"{Name} не стал покупать товар. Вероятность: {chance:F2}");
            }
        }
    }
}
