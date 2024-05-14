using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using AvaloniaApplication4.Views;
namespace AvaloniaApplication4.ViewModels;

public partial class CardOpenViewModel : ViewModelBase
{
   static MainWindowViewModel mainWindowViewModel = new();
    [ObservableProperty]
    private ViewModelBase _docViewModelBase = new DockPanelViewModel();

  
    [ObservableProperty]
    private ViewModelBase _tabViewModelBase = new TabViewModel(mainWindowViewModel);
}