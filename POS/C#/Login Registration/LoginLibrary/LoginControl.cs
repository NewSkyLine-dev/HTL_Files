using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LoginLibrary;

public class LoginControl : Control
{
    static LoginControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LoginControl), new FrameworkPropertyMetadata(typeof(LoginControl)));
    }

    public static readonly DependencyProperty IdentifierProperty =
        DependencyProperty.Register(nameof(Identifier), typeof(string), typeof(LoginControl));

    public static readonly DependencyProperty UseEmailProperty =
        DependencyProperty.Register(nameof(UseEmail), typeof(bool), typeof(LoginControl),
            new PropertyMetadata(true, OnUseEmailChanged));

    public static readonly DependencyProperty ErrorMessageProperty =
        DependencyProperty.Register(nameof(ErrorMessage), typeof(string), typeof(LoginControl));

    public static readonly DependencyProperty ErrorVisibilityProperty =
        DependencyProperty.Register(nameof(ErrorVisibility), typeof(Visibility), typeof(LoginControl),
            new PropertyMetadata(Visibility.Collapsed));

    public static readonly DependencyProperty IdentifierLabelProperty =
        DependencyProperty.Register(nameof(IdentifierLabel), typeof(string), typeof(LoginControl),
            new PropertyMetadata("Email:"));

    public static readonly RoutedEvent LoginEvent =
        EventManager.RegisterRoutedEvent(nameof(Login), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(LoginControl));

    public static readonly RoutedEvent SwitchToRegistrationEvent =
        EventManager.RegisterRoutedEvent(nameof(SwitchToRegistration), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(LoginControl));

    public event RoutedEventHandler Login
    {
        add => AddHandler(LoginEvent, value);
        remove => RemoveHandler(LoginEvent, value);
    }

    public event RoutedEventHandler SwitchToRegistration
    {
        add => AddHandler(SwitchToRegistrationEvent, value);
        remove => RemoveHandler(SwitchToRegistrationEvent, value);
    }

    public string Identifier
    {
        get => (string)GetValue(IdentifierProperty);
        set => SetValue(IdentifierProperty, value);
    }

    public bool UseEmail
    {
        get => (bool)GetValue(UseEmailProperty);
        set => SetValue(UseEmailProperty, value);
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

    private static void OnUseEmailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LoginControl control)
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

        if (GetTemplateChild("PART_LoginButton") is Button loginButton)
        {
            loginButton.Click += OnLogin;
        }

        if (GetTemplateChild("PART_SwitchToRegisterButton") is Button switchToRegistrationButton)
        {
            switchToRegistrationButton.Click += OnSwitchToRegistration;
        }

        if (GetTemplateChild("PART_SwitchIdentifier") is Button identifierButton)
        {
            identifierButton.Click += OnSwitchIdentifier;
        }
    }

    private void OnSwitchIdentifier(object sender, RoutedEventArgs e)
    {
        UseEmail = !UseEmail;
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

    private void OnLogin(object sender, RoutedEventArgs e)
    {
        if (ValidateForm())
        {
            RaiseEvent(new LoginRoutedEventArgs(LoginEvent, this, Identifier, Password));
        }
    }

    private void OnSwitchToRegistration(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(SwitchToRegistrationEvent, this));
    }
}

public class LoginRoutedEventArgs(RoutedEvent routedEvent, object source, string identifier, string password) : RoutedEventArgs(routedEvent, source)
{
    public string Identifier { get; } = identifier;
    public string Password { get; } = password;
}