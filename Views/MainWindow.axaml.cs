using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using StudentApp.ViewModels;

namespace StudentApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
