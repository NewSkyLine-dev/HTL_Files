using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PA1N_4A_2024
{
    internal class DayCheckout : Checkout
    {
        private readonly ManualResetEvent day = new(false);
        private readonly ManualResetEvent night = new(true);
        private readonly AutoResetEvent customerEvent = new(true);
        private readonly Thread? checkoutThread = null;

        public DayCheckout()
        {
            checkoutThread ??= new Thread(CheckoutTimer);
            checkoutThread.Start();
        }

        public override void checkout(Customer customer)
        {
            WaitHandle.WaitAll([day, night]);

            lock (this)
            {
                customerEvent.Set();
                MainWindow.MoveCustomer(customer, true);
                Thread.Sleep(customer.Items * 100);
                MainWindow.MoveCustomer(customer, false);
                customerEvent.Reset();
            }
        }

        private void CheckoutTimer()
        {
            while (true)
            {
                // Day: 10sec, Night: 5sec
                day.Set();
                Thread.Sleep(10000);
                day.Reset();

                customerEvent.WaitOne();

                night.Set();
                Thread.Sleep(5000);
                night.Reset();
            }
        }
    }
}
