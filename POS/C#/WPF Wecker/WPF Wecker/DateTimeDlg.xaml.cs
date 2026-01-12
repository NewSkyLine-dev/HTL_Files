using System.Windows;

namespace WPF_Wecker
{
    /// <summary>
    /// Interaction logic for DateTimeDlg.xaml
    /// </summary>
    public partial class DateTimeDlg : Window
    {
        DateTime alarmTime;

        public DateTime AlarmTime
        {
            get { return alarmTime; }
            set
            {
                alarmTime = value;
                this.dtp.Value = alarmTime;
            }
        }

        public DateTimeDlg()
        {
            InitializeComponent();

            dtp.Format =
                System.Windows.Forms.DateTimePickerFormat.Custom;
            dtp.CustomFormat = "ddd MMM dd yyyy - hh:mm:ss";
            dtp.ShowUpDown = false;
        }

        private void buttonOkay_Click(object sender, RoutedEventArgs e)
        {
            alarmTime = this.dtp.Value;
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
