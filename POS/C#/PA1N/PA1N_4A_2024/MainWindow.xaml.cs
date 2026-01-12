using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace PA1N_4A_2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Customer> customersQueue = [];
        public ObservableCollection<Customer> CustomersQueue
        {
            get { return customersQueue; }
            set { customersQueue = value; OnPropertyChanged(nameof(customersQueue)); }
        }

        private ObservableCollection<Customer> customersCheckout = [];
        public ObservableCollection<Customer> CustomersCheckout
        {
            get { return customersCheckout; }
            set { customersCheckout = value; OnPropertyChanged(nameof(customersCheckout)); }
        }


        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public static void MoveCustomer(Customer customer, bool toCheckout)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;

                if (toCheckout)
                {
                    window.CustomersQueue.Remove(customer);
                    window.CustomersCheckout.Add(customer);
                }
                else
                {
                    window.CustomersCheckout.Remove(customer);
                    //window.CustomersQueue.Add(customer);
                }
            });
        }

        public static void AddCustomerToQueue(Customer customer)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.CustomersQueue.Add(customer);
            });
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //Für die einzelnen Aufgaben wechseln
            Customer.Checkout = new DualCheckout();

            int customerCount = int.Parse(CustomerCount.Text);

            for (int i = 0; i < customerCount; i++)
            {
                Customer customer = new(i+1);
                //customersQueue.Add(customer);

                customer.StartThread();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}