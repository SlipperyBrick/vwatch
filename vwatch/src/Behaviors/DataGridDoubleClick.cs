using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace vwatch.Behaviors
{
    public static class DataGridDoubleClickBehavior
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(DataGridDoubleClickBehavior),
                new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                if (e.NewValue is ICommand)
                {
                    dataGrid.MouseDoubleClick += MouseDoubleClick;
                }
                else
                {
                    dataGrid.MouseDoubleClick -= MouseDoubleClick;
                }
            }
        }

        private static void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            if (dataGrid != null && GetCommand(dataGrid) is ICommand command)
            {
                var source = e.OriginalSource as DependencyObject;

                while (source != null && !(source is DataGridCell) && !(source is DataGridColumnHeader))
                {
                    source = VisualTreeHelper.GetParent(source);
                }

                if (source is DataGridCell || source is DataGridColumnHeader)
                {
                    return;
                }

                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }
        }

    }
}
