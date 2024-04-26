namespace vwatch.Behaviours
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public static class DeselectSelectedItem
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(DeselectSelectedItem),
                new PropertyMetadata(false, OnIsEnabledChanged));

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                if ((bool)e.NewValue)
                {
                    element.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
                }
                else
                {
                    element.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
                }
            }
        }

        private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement parent)
            {
                var listView = FindChild<ListView>(parent);
                if (listView != null && !IsClickOnListViewItem(listView, e))
                {
                    listView.SelectedItem = null;
                }
            }
        }

        private static bool IsClickOnListViewItem(ListView listView, MouseButtonEventArgs e)
        {
            var hitTestResult = VisualTreeHelper.HitTest(listView, e.GetPosition(listView));
            if (hitTestResult != null)
            {
                // Walk up the visual tree from the hit test result to find the ListViewItem
                DependencyObject current = hitTestResult.VisualHit;
                while (current != null && !(current is ListViewItem))
                {
                    current = VisualTreeHelper.GetParent(current);
                }

                if (current is ListViewItem item)
                {
                    // Check if the ListViewItem is part of the ListView's item collection
                    return item.DataContext != null;
                }
            }
            return false;
        }


        private static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild)
                {
                    foundChild = tChild;
                    break;
                }
                else
                {
                    foundChild = FindChild<T>(child);
                    if (foundChild != null) break;
                }
            }

            return foundChild;
        }
    }
}
