using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shell;

namespace FoodMarketDMS.Core.Controls
{
    public class ProgressWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _state;
        private int _progress;

        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
            }
        }


        public ProgressWindow()
        {
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Height = 100;
            Width = 300;

            //WindowStyle = WindowStyle.None;
            // replace WindowStyle.None to WindowChrome with 0 CaptionHeight
            // for borer effect.
            WindowChrome.SetWindowChrome(this, new WindowChrome() { CaptionHeight = 0 });

            Title = (string)Application.Current.Resources["Program_Name"];


            var textblock = new TextBlock()
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(4),
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(35, 15, 35, 60)
            };
            textblock.SetBinding(TextBlock.TextProperty, new Binding("State"));

            var progress = new ProgressBar()
            {
                Maximum = 100,
                Minimum = 0,
                Margin = new Thickness(35, 48, 35, 21)
            };
            progress.SetBinding(ProgressBar.ValueProperty, new Binding("Progress"));

            var grid = new Grid();
            grid.Children.Add(textblock);
            grid.Children.Add(progress);

            this.Content = grid;
            this.DataContext = this;
        }

    }
}
