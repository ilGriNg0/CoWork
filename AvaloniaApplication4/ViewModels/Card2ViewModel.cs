using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
namespace AvaloniaApplication4.ViewModels
{
    public partial class Card2ViewModel : ViewModelBase 
    {
        [ObservableProperty]
        private ViewModelBase _docViewModelBase3 = new DockPanel3ViewModel();

        [ObservableProperty]
        private ViewModelBase _tabViewModelBase = new TabViewModel();
    }
}
