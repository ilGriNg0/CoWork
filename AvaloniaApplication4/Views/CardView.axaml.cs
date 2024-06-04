using System;
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
        private void Border_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
           

            if (sender is Border bord && bord.DataContext is JsonClass js)
            {
                var viewModel = new DynamicCardsViewModel(js);
                ContentControl.Content = new DynamicCardsView {DataContext = viewModel };
            }

        }
    }
            
}
