using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplication4.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AvaloniaApplication4.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
        public MainWindow() : this(new MainWindowViewModel()) { }
        //private void Btn_OnClick(object? sender, RoutedEventArgs e)
        //{
        //    var main_page = new CardViewModel();
        //    Control_page.Content = main_page;
        //}
    }
}