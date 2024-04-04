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

namespace AvaloniaApplication4.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        ////public Bitmap? Image_bitmap_source { get; } = ImageLoad.Load("/Assets/prosto.jpg");
        [ObservableProperty]
        private ViewModelBase _page = new CardViewModel();

        [ObservableProperty]
        private bool is_open = true;

        [ObservableProperty]
        private CardModel? _cardOpen;

        [ObservableProperty] private IsMain? _isMain;
        partial void ismm(IsMain? ism);
        partial void ismm(IsMain? ism)
        {
            if (ism is null)
            {
                return;
            }

            var instance = Activator.CreateInstance(ism.Type);
            if (instance is null)
            {
                return;
            }

            _page = (ViewModelBase)instance;
        }
        public ObservableCollection<IsMain> _Mains { get; } = new()
        {
            new IsMain(typeof(CardViewModel))
        };
        //[RelayCommand]
        //private void back_page()
        //{
        //    MainWindow mnWindow = new();
        //    is_open = open.IsOpen;
        //    mnWindow.btn.IsVisible = !is_open;
        //    if (is_open)
        //    {
        //        is_open = !is_open;
        //    }
        //}
        //public ObservableCollection<CardModel> cards { get; } = new()
        //{
        //   new CardModel(typeof(CardViewModel))
        //};


        [RelayCommand]
        private void button_push()
        {
            var main_page = new CardViewModel();
            var main_ = new MainWindow();
            var main2 = new MainWindowViewModel();
            main2.Page = main_page;
            main_.Control_page.Content = main2.Page;
        }
    }
    public partial class IsMain 
    {
        public Type Type { get; }
        public string Title { get; }
        public IsMain(Type tp)
        {
            Type = tp;
            Title = tp.Name.Replace("ViewModel", "");
        }
    }
}


//< Image Grid.ColumnSpan = "2"
//Grid.RowSpan = "2"
//Source = "{Binding Image_bitmap_source}"
//Stretch = "Fill" ></ Image >