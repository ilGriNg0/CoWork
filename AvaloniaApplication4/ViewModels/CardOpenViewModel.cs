using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaApplication4.ViewModels;

public partial class CardOpenViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _docViewModelBase = new DockPanelViewModel();

    [ObservableProperty]
    private ViewModelBase _tabViewModelBase = new TabViewModel();
}