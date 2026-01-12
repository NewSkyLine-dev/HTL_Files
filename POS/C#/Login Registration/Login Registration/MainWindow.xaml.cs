using LoginLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Login_Registration;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private bool _useEmail;
    private Visibility _loginControlVisibility = Visibility.Visible;
    private Visibility _registrationControlVisibility = Visibility.Collapsed;

    public Visibility LoginControlVisibility
    {
        get => _loginControlVisibility;
        set
        {
            _loginControlVisibility = value;
            NotifyPropertyChanged();
        }
    }

    public Visibility RegistrationControlVisibility
    {
        get => _registrationControlVisibility;
        set
        {
            _registrationControlVisibility = value;
            NotifyPropertyChanged();
        }
    }

    public bool UseEmail
    {
        get { return _useEmail; }
        set { _useEmail = value; NotifyPropertyChanged(); }
    }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void loginControl_Login(object sender, RoutedEventArgs e)
    {
        if (e is LoginRoutedEventArgs loginArgs)
        {
            MessageBox.Show(
                $"Login Details:\nIdentifier: {loginArgs.Identifier}\nPassword: {loginArgs.Password}",
                "Login Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    private void loginControl_SwitchToRegistration(object sender, EventArgs e)
    {
        LoginControlVisibility = Visibility.Collapsed;
        RegistrationControlVisibility = Visibility.Visible;
    }

    private void registrationControl_Cancel(object sender, EventArgs e)
    {
        LoginControlVisibility = Visibility.Visible;
        RegistrationControlVisibility = Visibility.Collapsed;
    }

    private void registrationControl_Register(object sender, EventArgs e)
    {
        if (e is RegistrationRoutedEventArgs registerArgs)
        {
            MessageBox.Show(
                $"Registration Details:\nIdentifier: {registerArgs.Identifier}\nFirst Name: {registerArgs.FirstName}\nLast Name: {registerArgs.LastName}{$"{(registerArgs.Username != null ? $"\nUsername: {registerArgs.Username}" : string.Empty)}"}\nPassword: {registerArgs.Password}",
                "Registration Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            LoginControlVisibility = Visibility.Visible;
            RegistrationControlVisibility = Visibility.Collapsed;
        }
    }

    private void registrationControl_SwitchToLogin(object sender, EventArgs e)
    {
        LoginControlVisibility = Visibility.Visible;
        RegistrationControlVisibility = Visibility.Collapsed;
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}