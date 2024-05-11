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
using AvaloniaApplication4.Models;
using AvaloniaApplication4.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Npgsql;
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
        private ObservableCollection<ListItemTemplate>  items =
        [
           new ListItemTemplate(typeof(CardViewModel)),
           new ListItemTemplate(typeof(LoginViewModel)),
        ];

        //public ObservableCollection<ListItemTemplate> ItemTemplates { get; set; }

        
        [ObservableProperty]
        public ViewModelBase _currentPage = new CardViewModel();

        [ObservableProperty] 
        private ListItemTemplate? _selectedListItem;

        public ObservableCollection<ListItemTemplate> Items => items;
        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null) return;
            if (value.Instance is LoginViewModel && User.Model != null) CurrentPage = User.Model;
            else CurrentPage = (ViewModelBase)value.Instance;
        }
        public MainWindowViewModel()
        {
            User.Main = this;
        }
    }

        //    var instance = Activator.CreateInstance(value.Type);
        //    if (instance is null)
        //    {
        //        return;
        //    }

        //    Page = (ViewModelBase)instance;
        //}
        //public ObservableCollection<IsMain> Cards { get; } = new()
        //{
        //    new IsMain(typeof(CardViewModel)),

        //};
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


        //[RelayCommand]
        //private void button_push()
        //{
        //    var main_page = new CardViewModel();
        //    var main_ = new MainWindow();
        //    var main2 = new MainWindowViewModel();
        //    main2.Page = main_page;
        //    main_.Control_page.Content = main2.Page;
        //}
    }
    //public partial class IsMain 
    //{
    //    public Type Type { get; }
    //    public string Title { get; }
    //    public IsMain(Type tp)
    //    {
    //        Type = tp;
    //        Title = tp.Name.Replace("ViewModel", "");
    //    }
    //}
}


//< Image Grid.ColumnSpan = "2"
//Grid.RowSpan = "2"
//Source = "{Binding Image_bitmap_source}"
//Stretch = "Fill" ></ Image >