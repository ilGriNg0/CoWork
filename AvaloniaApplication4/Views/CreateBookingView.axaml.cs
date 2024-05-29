using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.ViewModels;

namespace AvaloniaApplication4.Views
{
    public partial class CreateBookingView : UserControl
    {
        public CreateBookingView()
        {
            InitializeComponent();
        }
        private void Exit_Click(object source, RoutedEventArgs args)
        {
            var parent = this.Parent as ContentControl;
            if (parent != null)
            {
                parent.Content = null; // Убираем содержимое при закрытии
            }
        }
    }
}
