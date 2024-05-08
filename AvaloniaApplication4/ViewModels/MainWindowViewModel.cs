using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Reactive;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using Avalonia.Skia.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.ViewModels;
using CommunityToolkit.Mvvm.Input;
using AvaloniaApplication4.Views;
using System.Windows.Input;

namespace AvaloniaApplication4.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        ////public Bitmap? Image_bitmap_source { get; } = ImageLoad.Load("/Assets/prosto.jpg");
        [ObservableProperty]
        private ViewModelBase _page = new CardViewModel();
       
        [ObservableProperty]
        private static bool is_open = true;

        [ObservableProperty]
        private CardModel? _cardOpen;

       
        private static string Namespace()
        {
            Type tp = typeof(MainWindowViewModel);
            string yNameSpc = tp.Namespace;
            return yNameSpc;
        }
        public ICommand NavigateCommand => new RelayCommand<string>(Navigate);

  
        public void Navigate(string? pageViewModel)
        {
            string namspc = Namespace();
            Type viewModelType = Type.GetType(namspc + "." + pageViewModel);
         
            if (viewModelType != null)
            {
                ViewModelBase viewModel = (ViewModelBase)Activator.CreateInstance(viewModelType);
                Page = viewModel;
            }
        }

       
       
    }
   public partial  class JsonClass : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string? _name_cowork;

        [ObservableProperty]

        private string? _info_cowork;
        [ObservableProperty]
        private string? _location_metro_cowork;
        [ObservableProperty]
        private string? _price_day_cowork;
        [ObservableProperty]
        private string? _price_meetingroom_cowork;
    


    }
}


//< Image Grid.ColumnSpan = "2"
//Grid.RowSpan = "2"
//Source = "{Binding Image_bitmap_source}"
//Stretch = "Fill" ></ Image >