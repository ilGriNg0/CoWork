using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using Avalonia.Skia.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using Npgsql;
using ReactiveUI;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.ViewModels;
using CommunityToolkit.Mvvm.Input;
using AvaloniaApplication4.Views;
using System.Windows.Input;
using Avalonia.Controls;
using System.Reflection.Metadata.Ecma335;

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

        [ObservableProperty]
        private string? _content;

        
        public void update(string? content)
        {
            string namspc = Namespace();
            Type viewModelType = Type.GetType(namspc + "." + content);
            ViewModelBase viewModel = (ViewModelBase)Activator.CreateInstance(viewModelType);
            Page = null;

        }
        private static MainWindowViewModel? _instance;
       
        public static MainWindowViewModel Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new MainWindowViewModel();
                }
                return _instance;
            }
         

        }
  

        public  string Namespace()
        {
            Type tp = typeof(MainWindowViewModel);
            string? yNameSpc = tp.Namespace;
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
   public partial class JsonClass : ObservableObject
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
        [ObservableProperty]
        private string? _path_photo;

    }

    
}


//< Image Grid.ColumnSpan = "2"
//Grid.RowSpan = "2"
//Source = "{Binding Image_bitmap_source}"
//Stretch = "Fill" ></ Image >