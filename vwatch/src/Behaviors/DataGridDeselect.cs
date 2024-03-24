using System.Windows.Input;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace vwatch.Behaviors
{
    public class DataGridDeselect : Behavior<Window>
    {
        public static readonly DependencyProperty DeselectCommandProperty =
            DependencyProperty.Register(
                "DeselectCommand",
                typeof(ICommand),
                typeof(DataGridDeselect),
                new PropertyMetadata(default(ICommand)));

        public ICommand DeselectCommand
        {
            get { return (ICommand)GetValue(DeselectCommandProperty); }
            set { SetValue(DeselectCommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewMouseDown += OnPreviewMouseDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewMouseDown -= OnPreviewMouseDown;
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DeselectCommand != null && DeselectCommand.CanExecute(null))
            {
                DeselectCommand.Execute(null);
            }
        }
    }

}
