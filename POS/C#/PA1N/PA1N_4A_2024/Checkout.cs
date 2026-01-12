using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA1N_4A_2024
{
    public class Checkout
    {
        public virtual void checkout(Customer customer)
        {
            lock (this)
            {
                MainWindow.MoveCustomer(customer, true);
                Thread.Sleep(customer.Items * 100);
                MainWindow.MoveCustomer(customer, false);
            }
        }
    }
}
