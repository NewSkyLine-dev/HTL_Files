using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PA1N_4A_2024
{
    internal class FastCheckout : Checkout
    {
        private static readonly SemaphoreSlim semaphore = new(2);
        private static object lockObj = new();
        private static int? currentCheckout = null;
        private List<Customer> waiting = [];

        public override void checkout(Customer customer)
        {
            waiting.Add(customer);

            semaphore.Wait();

            if (customer.Items >= 8 && waiting.Count > 1)
            {
                lock (lockObj)
                {
                    waiting.Sort((a, b) => a.Items.CompareTo(b.Items));
                    var index = waiting.FindIndex(c => c == customer);
                    if (index > 0)
                    {
                        waiting.RemoveAt(index);
                        waiting.Insert(0, customer);
                    }
                }
            }

            lock (lockObj)
            {
                while (currentCheckout != null && currentCheckout > 2)
                {
                    Monitor.Wait(lockObj);
                }
                if (currentCheckout == null)
                {
                    currentCheckout = 1;
                    customer.CheckoutNumber = (int)currentCheckout;
                }
                else if (currentCheckout == 1)
                {
                    currentCheckout = 2;
                    customer.CheckoutNumber = (int)currentCheckout;
                    currentCheckout = null;
                }
            }

            MainWindow.MoveCustomer(customer, true);
            Thread.Sleep(customer.Items * 100);
            MainWindow.MoveCustomer(customer, false);

            semaphore.Release();

            lock (lockObj)
            {
                if (semaphore.CurrentCount == 2)
                {
                    currentCheckout = 1;
                    Monitor.PulseAll(lockObj);
                }
            }
        }
    }
}
