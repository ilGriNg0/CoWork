using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplication4.ViewModels;
namespace AvaloniaApplication4.Views;

public partial class DynamicCardsView : UserControl
{
    public DynamicCardsView()
    {
        InitializeComponent();
        this.GetControl<ScrollViewer>("Scroll").ScrollToHome();
    }

    private void SelectedBook_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Parent.Parent is Border bord && bord.DataContext is TariffElements tf)
        {
            CreateBookingViewModel.choise = tf.Tarif_id;
        }
    }
}