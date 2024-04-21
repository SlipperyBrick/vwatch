using System.Windows;
using vwatch.ViewModels;

namespace vwatch
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        public ConfigurationWindow(ConfigurationWindowViewModel viewModel)
        {
            InitializeComponent();

            this.DataContext = viewModel;

            viewModel.RequestClose += (sender, e) => this.Close();
        }
    }
}
