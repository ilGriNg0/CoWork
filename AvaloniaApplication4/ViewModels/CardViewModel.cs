
using Avalonia.Controls.Documents;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaApplication4.ViewModels;

public partial class CardViewModel : ViewModelBase
{
    //[ObservableProperty]
    //private ViewModelBase _open = new CardOpenViewModel();
    [ObservableProperty]
    private ViewModelBase _base = new CardOpenViewModel();

    [ObservableProperty]
    private bool _isOpen = true;
}