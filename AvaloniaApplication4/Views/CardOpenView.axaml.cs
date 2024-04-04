using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaApplication4.Views
{
    public partial class CardOpenView : UserControl
    {
        public CardOpenView()
        {
            InitializeComponent();
        }

        private void Button_OnClick_Previous(object? sender, RoutedEventArgs e)
        {
            carousel.Previous();
        }

        private void Button_OnClick_Next(object? sender, RoutedEventArgs e)
        {
           carousel.Next();
        }
    }
}
