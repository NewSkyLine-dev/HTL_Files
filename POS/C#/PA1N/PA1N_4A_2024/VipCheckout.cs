namespace PA1N_4A_2024
{
    internal class VipCheckout : Checkout
    {
        private readonly PriorityQueue<Customer, bool> customers = new();
        private static readonly SemaphoreSlim semaphore = new(2);
        private static readonly object lockObj = new();
        private static int? currentCheckout = null;

        public override void checkout(Customer customer)
        {
            lock (lockObj)
            {
                customers.Enqueue(customer, customer.Vip);
            }

            semaphore.Wait();

            lock (lockObj)
            {
                while (currentCheckout != null && currentCheckout > 2)
                {
                    Monitor.Wait(lockObj);
                }
                if (currentCheckout == null)
                {
                    currentCheckout = 1;
                    customers.Peek().CheckoutNumber = (int)currentCheckout;
                }
                else if (currentCheckout == 1)
                {
                    currentCheckout = 2;
                    customers.Peek().CheckoutNumber = (int)currentCheckout;
                    currentCheckout = null;
                }
            }

            MainWindow.MoveCustomer(customers.Dequeue(), true);
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
