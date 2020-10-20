using System.Windows;

namespace FoodMarketDMS.Core.Mvvm
{
    public interface IWindowLoadedLoader
    {
        void WindowLoaded();
    }

    public class WindowLoadedLoader
    {
        public static bool GetEnableWindowLoadedLoader(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableWindowLoadedLoaderProperty);
        }

        public static void SetEnableWindowLoadedLoader(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableWindowLoadedLoaderProperty, value);
        }

        public static readonly DependencyProperty EnableWindowLoadedLoaderProperty =
            DependencyProperty.RegisterAttached("EnableWindowLoadedLoader", typeof(bool), typeof(WindowLoadedLoader), new PropertyMetadata(false, OnEnableWindowLoadedLoaderChanged));

        private static void OnEnableWindowLoadedLoaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Window window)
            {
                window.Loaded += (s, e) =>
                {
                    if (window.DataContext is IWindowLoadedLoader vm)
                    {
                        vm.WindowLoaded();
                    }
                };
            }
        }
    }
}
