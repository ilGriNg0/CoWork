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
            var mn = new MainWindow();
            //var mm = new isMain();
            bool opn;
            if (Bord.IsEnabled)
            {
                //viewModel.IsOpen = false;
                ContentControl.Content = viewModel?.Base;
                //mm.ViewModel.Ism = new MainWindowViewModel();
                //mm.ViewModel.Ism.Is_open = false;
                //mm.Is_open = false;
                //opn = mm.Is_open;
                ////mn.btn.IsVisible = false;
                //new isMain(mm.ViewModel, opn);
            }

        }



    }
            
}
