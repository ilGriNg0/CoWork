using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
namespace AvaloniaApplication4.Views
{
    public partial class CardView : UserControl
    {
        public CardView()
        {
            InitializeComponent();
        }

        private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var viewModel = new CardViewModel();
            var mm = new MainWindowViewModel();

            
                ContentControl.Content = viewModel?.Base;

            

        }
        private void Border2_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var viewModel = new CardViewModel();



            ContentControl.Content = viewModel?.Base2_card;



        }
        private void Border3_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var viewModel = new CardViewModel();


            ContentControl.Content = viewModel?.Base3_card;



        }


    }
            
}
