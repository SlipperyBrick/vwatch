using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace vwatch.Behaviors
{
    public class ConfigurationWindowClose : Behavior<Button>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Click += OnButtonClick;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Click -= OnButtonClick;
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this.AssociatedObject);
            window?.Close();
        }
    }
}
