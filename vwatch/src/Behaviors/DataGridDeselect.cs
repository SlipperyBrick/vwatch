using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

using vwatch.ViewModels;

namespace vwatch.Behaviors
{
    public class DataGridDeselect : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewMouseUp += OnPreviewMouseUp;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.PreviewMouseUp -= OnPreviewMouseUp;
            base.OnDetaching();
        }

        private void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Check if the click is outside the DataGrid items and specific controls like ComboBox.
            if (!(e.OriginalSource is DependencyObject source)) return;

            // Determine if the clicked source is part of DataGrid or a specific control that should not trigger deselection.
            var isClickInsideDataGridItems = FindParent<DataGridRow>(source) != null;
            var isClickInsideComboBox = FindParent<ComboBox>(source) != null;

            if (!isClickInsideDataGridItems && !isClickInsideComboBox)
            {
                // Attempt to find a DataGrid and its ViewModel in the visual tree.
                var dataGrid = FindParent<DataGrid>(source);
                if (dataGrid?.DataContext is DataGridViewModel viewModel)
                {
                    viewModel.ClearSelection();

                    dataGrid.UnselectAll();

                    // Close any open ComboBox dropdown within the DataGrid.
                    CloseOpenComboBoxes(dataGrid);

                    // Optionally, reset focus to the DataGrid or another neutral control.
                    Keyboard.Focus(null);
                }
            }
        }

        private void CloseOpenComboBoxes(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is ComboBox comboBox && comboBox.IsDropDownOpen)
                {
                    comboBox.IsDropDownOpen = false;
                    comboBox.SelectedItem = null;
                    comboBox.SelectedValue = -1;
                }
                else
                {
                    CloseOpenComboBoxes(child); // Recursively check all children.
                }
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject current = child;
            while (current != null)
            {
                if (current is T found)
                {
                    return found;
                }
                current = VisualTreeHelper.GetParent(current) ?? (current as FrameworkElement)?.Parent;
            }
            return null;
        }
    }
}
