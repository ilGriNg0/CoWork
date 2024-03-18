using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
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
            if (Bord.IsEnabled)
            {
                viewModel.IsOpen = false;
                ContentControl.Content = viewModel?.Base;
            }

        }



    }
            
}
