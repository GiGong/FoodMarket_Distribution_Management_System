using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodMarketDMS.Core.Controls
{
    public class CommandListView : ListView
    {

        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(CommandListView), new PropertyMetadata(null));


        public ICommand DoubleClickCommand
        {
            get { return (ICommand)GetValue(DoubleClickCommandProperty); }
            set { SetValue(DoubleClickCommandProperty, value); }
        }


        public CommandListView()
        {
            Style itemStyle = new Style(typeof(ListViewItem));
            EventSetter eventSetter = new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(ItemMouseDoubleClick));
            itemStyle.Setters.Add(eventSetter);
            ItemContainerStyle = itemStyle;
        }

        private void ItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is ListViewItem item))
            {
                return;
            }

            if (DoubleClickCommand != null && DoubleClickCommand.CanExecute(item.DataContext))
            {
                DoubleClickCommand.Execute(item.DataContext);
            }
        }
    }
}
