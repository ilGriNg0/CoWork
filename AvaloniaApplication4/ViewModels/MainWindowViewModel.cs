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
using Avalonia.Controls;
using System.Reflection.Metadata.Ecma335;
using static AvaloniaApplication4.ViewModels.JsonClass;
using Avalonia.Media;

namespace AvaloniaApplication4.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _page = new CardViewModel();


        //public ObservableCollection<ListItemTemplate> Items => items;
        //private readonly ObservableCollection<ListItemTemplate> items =
        //[
        //   new ListItemTemplate(typeof(CardViewModel)),
        //   new ListItemTemplate(typeof(LoginViewModel)),
        //];

        ////public Bitmap? Image_bitmap_source { get; } = ImageLoad.Load("/Assets/prosto.jpg");

        [ObservableProperty]
        private ListItemTemplate? _selectedListItem;

        [ObservableProperty]
        private string? _content;

        public string Namespace()
        {
            Type tp = typeof(MainWindowViewModel);
            string? yNameSpc = tp.Namespace;
            return yNameSpc;
        }
        public ICommand NavigateCommand => new RelayCommand<string>(Navigate);

        public MainWindowViewModel()
        {
            User.Main = this;
            User.Model = new LoginViewModel();
        }
        [ObservableProperty]
        private SolidColorBrush _color1 = new SolidColorBrush(Color.Parse("#D94D04"));
        [ObservableProperty]
        private SolidColorBrush _color2 = new SolidColorBrush(Colors.Black);

        public void Navigate(string? pageViewModel)
        {
            string namspc = Namespace();
            Type viewModelType = Type.GetType(namspc + "." + pageViewModel);

            if (pageViewModel == "LoginViewModel" && Color1.Color == Colors.Black) return;
            else if (pageViewModel == "LoginViewModel")
            {
                var bufer = Color1;
                Color1 = Color2;
                Color2 = bufer;
                Page = User.Model;
            }
            else
            {
                if (Color1.Color == Colors.Black)
                {
                    var bufer = Color1;
                    Color1 = Color2;
                    Color2 = bufer;
                }
                
                ViewModelBase viewModel = (ViewModelBase)Activator.CreateInstance(viewModelType);
                Page = viewModel;
            }
        }

       
        //public void update(string? content);
        //public ObservableCollection<ListItemTemplate> Items => items;
        //partial void OnSelectedListItemChanged(ListItemTemplate? value)
        //{
        //    if (value is null) return;
        //    if (value.Instance is LoginViewModel && User.Model != null) CurrentPage = User.Model;
        //    else CurrentPage = (ViewModelBase)value.Instance;
        //    string namspc = Namespace();
        //    Type viewModelType = Type.GetType(namspc + "." + content);
        //    ViewModelBase viewModel = (ViewModelBase)Activator.CreateInstance(viewModelType);
        //    Page = null;

        //}
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
  

       
        //public MainWindowViewModel()
        //{
        //    User.Main = this;
        //}
    
  
        

       
       
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

        public class ListItemTemplate(Type type)
        {
            public object Instance { get; } = Activator.CreateInstance(type);
            public string Label { get; } = type.Name.Replace("ViewModel", "");
            public Type ModelType { get; } = type;
            //    MainWindow mnWindow = new();
        }


    }

     public partial class IdCompany : ObservableObject
    {
        [ObservableProperty]
        private int _id_Company;
    }
//< Image Grid.ColumnSpan = "2"
//Grid.RowSpan = "2"
//Source = "{Binding Image_bitmap_source}"
//Stretch = "Fill" ></ Image >        //};


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


//< Image Grid.ColumnSpan = "2"
//Grid.RowSpan = "2"
//Source = "{Binding Image_bitmap_source}"
//Stretch = "Fill" ></ Image >