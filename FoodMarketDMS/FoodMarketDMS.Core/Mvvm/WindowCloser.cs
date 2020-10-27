using System;
using System.ComponentModel;
using System.Windows;

namespace FoodMarketDMS.Core.Mvvm
{
    public interface IClosingWindow
    {
        Action Close { get; set; }
        void WindowClosing(object sender, CancelEventArgs e);
    }

    public class WindowCloser
    {
        public static bool GetEnableWindowCloser(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableWindowCloserProperty);
        }

        public static void SetEnableWindowCloser(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableWindowCloserProperty, value);
        }

        public static readonly DependencyProperty EnableWindowCloserProperty =
            DependencyProperty.RegisterAttached("EnableWindowCloser", typeof(bool), typeof(WindowCloser), new PropertyMetadata(false, OnEnableWindowCloserChanged));

        private static void OnEnableWindowCloserChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Window window)
            {
                window.Loaded += (s, e) =>
                {
                    if (window.DataContext is IClosingWindow vm)
                    {
                        vm.Close += () =>
                        {
                            window.Close();
                        };

                        window.Closing += vm.WindowClosing;
                    }
                };
            }
        }
    }

}
