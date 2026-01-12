using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA1N_4A_2024
{
    public class Customer
    {
        public static Random random = new Random();
        public static Checkout Checkout {  get; set; } = new Checkout();
        public int Items { get; set; } = random.Next(4, 30);
        public bool Vip {  get; set; } = random.Next(100) > 95;
        public int CheckoutNumber { get; set; } = 0;
        public int Id { get; set; }
        private Thread thread;

        public Customer(int id) 
        { 
            Id = id;
        }

        public void Shop()
        { 
            Thread.Sleep(random.Next(1000, 10001));
            MainWindow.AddCustomerToQueue(this);
            Checkout.checkout(this);
        }

        public override string ToString()
        {
            return String.Format("Customer: {0} Items: {1} {2}{3}", Id, Items, Vip ? "VIP" : "", CheckoutNumber == 0 ? "" : "Checkout-Nr.: " + CheckoutNumber);
        }

        internal void StartThread()
        {
            thread = new Thread(Shop) { Name = $"Customer {Id}" };
            thread.Start();
        }
    }
}
