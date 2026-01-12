using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LoginLibrary;

public class RegistrationControl : Control
{
    static RegistrationControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RegistrationControl), new FrameworkPropertyMetadata(typeof(RegistrationControl)));
    }

    #region Dependency Properties
    public static readonly DependencyProperty IdentifierProperty =
        DependencyProperty.Register(nameof(Identifier), typeof(string), typeof(RegistrationControl));

    public static readonly DependencyProperty FirstNameProperty =
        DependencyProperty.Register(nameof(FirstName), typeof(string), typeof(RegistrationControl));

    public static readonly DependencyProperty LastNameProperty =
        DependencyProperty.Register(nameof(LastName), typeof(string), typeof(RegistrationControl));

    public static readonly DependencyProperty IdentifierLabelProperty =
        DependencyProperty.Register(nameof(IdentifierLabel), typeof(string), typeof(RegistrationControl),
            new PropertyMetadata("Email:"));

    public static readonly DependencyProperty AddressProperty =
        DependencyProperty.Register(nameof(Address), typeof(string), typeof(RegistrationControl));
    
    public static readonly DependencyProperty UsernameVisibilityProperty =
        DependencyProperty.Register(nameof(UsernameVisibility), typeof(Visibility), typeof(RegistrationControl));

    public static readonly DependencyProperty UsernameProperty =
        DependencyProperty.Register(nameof(Username), typeof(string), typeof(RegistrationControl));

    public static readonly DependencyProperty ErrorMessageProperty =
        DependencyProperty.Register(nameof(ErrorMessage), typeof(string), typeof(RegistrationControl));

    public static readonly DependencyProperty ErrorVisibilityProperty =
        DependencyProperty.Register(nameof(ErrorVisibility), typeof(Visibility), typeof(RegistrationControl),
            new PropertyMetadata(Visibility.Collapsed));

    public static readonly DependencyProperty UseEmailProperty =
        DependencyProperty.Register(nameof(UseEmail), typeof(bool), typeof(RegistrationControl),
            new PropertyMetadata(false, OnUseEmailChanged));

    public static readonly RoutedEvent RegisterEvent =
        EventManager.RegisterRoutedEvent(nameof(Register), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(RegistrationControl));

    public static readonly RoutedEvent ResetEvent =
        EventManager.RegisterRoutedEvent(nameof(Reset), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(RegistrationControl));

    public static readonly RoutedEvent CancelEvent =
        EventManager.RegisterRoutedEvent(nameof(Cancel), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(RegistrationControl));

    public static readonly RoutedEvent SwitchToLoginEvent =
        EventManager.RegisterRoutedEvent(nameof(SwitchToLogin), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(RegistrationControl));

    public event RoutedEventHandler Register
    {
        add => AddHandler(RegisterEvent, value);
        remove => RemoveHandler(RegisterEvent, value);
    }

    public event RoutedEventHandler Reset
    {
        add => AddHandler(ResetEvent, value);
        remove => RemoveHandler(ResetEvent, value);
    }

    public event RoutedEventHandler Cancel
    {
        add => AddHandler(CancelEvent, value);
        remove => RemoveHandler(CancelEvent, value);
    }

    public event RoutedEventHandler SwitchToLogin
    {
        add => AddHandler(SwitchToLoginEvent, value);
        remove => RemoveHandler(SwitchToLoginEvent, value);
    }

    public string Identifier
    {
        get => (string)GetValue(IdentifierProperty);
        set => SetValue(IdentifierProperty, value);
    }

    public Visibility UsernameVisibility
    {
        get => (Visibility)GetValue(UsernameVisibilityProperty);
        set => SetValue(UsernameVisibilityProperty, value);
    }

    public string Username
    {
        get => (string)GetValue(UsernameProperty);
        set => SetValue(UsernameProperty, value);
    }

    public string FirstName
    {
        get => (string)GetValue(FirstNameProperty);
        set => SetValue(FirstNameProperty, value);
    }

    public string LastName
    {
        get => (string)GetValue(LastNameProperty);
        set => SetValue(LastNameProperty, value);
    }

    public bool UseEmail
    {
        get => (bool)GetValue(UseEmailProperty);
        set => SetValue(UseEmailProperty, value);
    }

    public string Address
    {
        get => (string)GetValue(AddressProperty);
        set => SetValue(AddressProperty, value);
    }

    public string ErrorMessage
    {
        get => (string)GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
    }

    public Visibility ErrorVisibility
    {
        get => (Visibility)GetValue(ErrorVisibilityProperty);
        set => SetValue(ErrorVisibilityProperty, value);
    }

    public string IdentifierLabel
    {
        get => (string)GetValue(IdentifierLabelProperty);
        set => SetValue(IdentifierLabelProperty, value);
    }

    public string Password { get; private set; } = string.Empty;
    #endregion

    private static void OnUseEmailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RegistrationControl control)
        {
            control.IdentifierLabel = control.UseEmail ? "Email:" : "Username:";
        }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        if (GetTemplateChild("PART_PasswordBox") is PasswordBox passwordBox)
        {
            passwordBox.PasswordChanged += (s, e) => Password = passwordBox.Password;
        }

        if (GetTemplateChild("PART_RegisterButton") is Button registerButton)
        {
            registerButton.Click += OnRegister;
        }

        if (GetTemplateChild("PART_ResetButton") is Button resetButton)
        {
            resetButton.Click += OnReset;
        }

        if (GetTemplateChild("PART_CancelButton") is Button cancelButton)
        {
            cancelButton.Click += OnCancel;
        }

        if (GetTemplateChild("PART_SwitchToLogin") is Button switchToLoginButton)
        {
            switchToLoginButton.Click += OnSwitchToLogin;
        }

        if (GetTemplateChild("Part_SwitchIdentifier") is Button identifierButton)
        {
            identifierButton.Click += (s, e) => UseEmail = !UseEmail;
        }
    }
    private void OnRegister(object sender, RoutedEventArgs e)
    {
        // Check with regex if email is correct
        if (ValidateForm())
        {
            RaiseEvent(new RegistrationRoutedEventArgs(RegisterEvent, this, Identifier, FirstName, LastName, Password, (UsernameVisibility != Visibility.Collapsed ? Username : null)));
        }
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(Identifier))
        {
            ErrorMessage = UseEmail ? "Email is required" : "Username is required";
            ErrorVisibility = Visibility.Visible;
            return false;
        }

        if (UseEmail && !Regex.IsMatch(Identifier, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
        {
            ErrorMessage = "Invalid email format";
            ErrorVisibility = Visibility.Visible;
            return false;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Password is required";
            ErrorVisibility = Visibility.Visible;
            return false;
        }

        ErrorVisibility = Visibility.Collapsed;
        return true;
    }

    private void OnReset(object sender, RoutedEventArgs e)
    {
        Username = string.Empty;
        Identifier = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Password = string.Empty;
        ErrorVisibility = Visibility.Collapsed;
        RaiseEvent(new RoutedEventArgs(ResetEvent, this));
    }

    private void OnCancel(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(CancelEvent, this));

    private void OnSwitchToLogin(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(SwitchToLoginEvent, this));
}

public class RegistrationRoutedEventArgs(RoutedEvent routedEvent, object source, string identifier, string firstName, string lastName, string password, string? username) : RoutedEventArgs(routedEvent, source)
{
    public string Identifier { get; } = identifier;
    public string Password { get; } = password;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public string? Username { get; } = username;
}