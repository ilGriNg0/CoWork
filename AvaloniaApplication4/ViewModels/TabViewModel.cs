using Avalonia.Controls;
using AvaloniaApplication4.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using System.Xml.Serialization;

namespace AvaloniaApplication4.ViewModels;

public partial class TabViewModel : ViewModelBase
{
    public static MainWindowViewModel mainWindowViewModel1;
 
    public ICommand ReqUpdCommand { get; set; }
    public TabViewModel(MainWindowViewModel mainWindowViewModel)
    {
        mainWindowViewModel1 = mainWindowViewModel;
        ReqUpdCommand = new RelayCommand(ReqUpd);
    }
    private void ReqUpd()
    {
        mainWindowViewModel1.update("CardViewModel");
    }
    public TabViewModel() { }
    public ICommand NavigateCardsCommand => new RelayCommand<string>(NavigateCards);
    private static void NavigateCards(string? parametr)
    {
        MainWindowViewModel mainWindowViewModel = new();
        string namspc = mainWindowViewModel.Namespace();
        Type viewModelType = Type.GetType(namspc + "." + parametr);

        if (viewModelType != null)
        {
            var viewModel = Activator.CreateInstance(viewModelType);
            if(viewModel is ViewModelBase vm) 
            {
                mainWindowViewModel.Page = (ViewModelBase)viewModel;
            }
          
           
        }

    }
    //TabView tb = new();
    //[ObservableProperty] private string _NameButton;

    //[ObservableProperty] private ViewModelBase _view;

    //[ObservableProperty] private MainWindow _main;
    //private int counter;

    //public TabViewModel()
    //{
    //    IncrementCounterCommand = new RelayCommand(IncrementCounter);
    //    _main = new MainWindow();
    //    _main.Control_page.Content  = this; 
    //}


    //public int Counter
    //{
    //    get => counter;
    //    private set => SetProperty(ref counter, value);
    //}

    //public ICommand IncrementCounterCommand { get; }

    //private void IncrementCounter() 
    //{
    //    _main.Control_page.Content = View;
    //}

    //[RelayCommand]
    //private void OpenCard()
    //{
    //    //MainWindowViewModel viewModel = new MainWindowViewModel();
    //    MainWindow mainWindow = new();
    //    TabViewModel tabViewModel = new(); 

    //}
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler? CanExecuteChanged;
    }

}