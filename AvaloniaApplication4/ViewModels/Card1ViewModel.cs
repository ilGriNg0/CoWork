﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication4.ViewModels
{
    public partial class Card1ViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _docViewModelBase2 = new DockPanel2ViewModel();

        [ObservableProperty]
        private ViewModelBase _tabViewModelBase = new TabViewModel();
    }
}