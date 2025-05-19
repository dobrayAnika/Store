using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;

namespace StoreApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // Обработчик события изменения размеров окна
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var canvas = this.FindControl<Canvas>("MainCanvas");
            if (canvas == null)
                return;

            var maxX = canvas.Children.OfType<Control>()
                            .Max(c => Canvas.GetLeft(c) + c.Bounds.Width);
            var maxY = canvas.Children.OfType<Control>()
                            .Max(c => Canvas.GetTop(c) + c.Bounds.Height);

            canvas.Width = Math.Max(maxX, canvas.Width);
            canvas.Height = Math.Max(maxY, canvas.Height);
        }
    }
}
