using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApplication4.ViewModels;

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
            slides.Previous();
        }

        private void Button_OnClick_Next(object? sender, RoutedEventArgs e)
        {
            slides.Next();
        }
    }
}
