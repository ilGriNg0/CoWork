using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.ViewModels;

namespace AvaloniaApplication4.Views
{
    public partial class PersonalAccountView : UserControl
    {
        public PersonalAccountView()
        {
            InitializeComponent();
        }

        private void Exit_Click(object source, RoutedEventArgs args)
        {
            User.Main.CurrentPage = new LoginViewModel();
        }

        private void Change_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            return;
        }
    }
}
