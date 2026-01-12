using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    public class UserIcon : Control
    {
        #region Dependency Properties
        public static readonly DependencyProperty UserIconColorProperty =
            DependencyProperty.Register(
                "UserIconColor",
                typeof(SolidColorBrush),
                typeof(UserIcon),
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.None, null));
        public SolidColorBrush UserIconColor
        {
            get => (SolidColorBrush)GetValue(UserIconColorProperty);
            set => SetValue(UserIconColorProperty, value);
        }

        public static readonly DependencyProperty UserIconNameProperty =
            DependencyProperty.Register(
                "UserIconName",
                typeof(string),
                typeof(UserIcon),
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.None, null));
        public string UserIconName
        {
            get => (string)GetValue(UserIconNameProperty);
            set => SetValue(UserIconNameProperty, value);
        }
        #endregion

        static UserIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserIcon), new FrameworkPropertyMetadata(typeof(UserIcon)));
        }
    }
}
