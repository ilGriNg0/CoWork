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
        [ObservableProperty]
        private static bool _clicked_navigate;
        /// <summary>
        /// Рефакторинг #1 :
        ///  Обратить внимание на создание нового экземпляра PersonalAccountViewModel 
        ///  Нужно реорганизовать метод
        /// </summary>
        /// <param name="Navigate"></param>
        public void Navigate(string? pageViewModel)
        {
            string namspc = Namespace();
            PersonalAccountViewModel personalAccountViewModel = new();
            Clicked_navigate = personalAccountViewModel.Pressed;
            Type viewModelType = Type.GetType(namspc + "." + pageViewModel);
            bool clicked = true;
            if (pageViewModel == "LoginViewModel" && Color1.Color == Colors.Black && Page == User.Model)
            {
                
                if ( Clicked_navigate)
                { 
                    Type viewModelType2 = Type.GetType(namspc + "." + "PersonalAccountViewModel");
                    ViewModelBase viewModel2 = (ViewModelBase)Activator.CreateInstance(viewModelType2);
                    Page = viewModel2;
                    Clicked_navigate = false;
                    personalAccountViewModel.Pressed = false;
                }
                    
                
               
            }
            else if (pageViewModel == "LoginViewModel")
            {
                if (Color1.Color != Colors.Black)
                {
                    var bufer = Color1;
                    Color1 = Color2;
                    Color2 = bufer;
                }
                    Page = User.Model;
                
            }
            else
            {
                if (Color1.Color == Colors.Black && viewModelType == typeof(CardViewModel))
                {
                    var bufer = Color1;
                    Color1 = Color2;
                    Color2 = bufer;
                }
                
                ViewModelBase viewModel = (ViewModelBase)Activator.CreateInstance(viewModelType);
                Page = viewModel;
                clicked = true;
              
            }
         
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
        //public MainWindowViewModel()
        //{
        //    User.Main = this;
        //}
    }
   public partial class JsonClass :ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string? _name_cowork;

        [ObservableProperty]

        private string? _info_cowork;
        [ObservableProperty]
        private string? _location_cowork;
        [ObservableProperty]
        private string? _price_day_cowork;
        [ObservableProperty]
        private string? _price_meetingroom_cowork;
        [ObservableProperty]
        private Bitmap?  _path_photo;
        [ObservableProperty]
        private string? _date_created;
        [ObservableProperty]
        private string? _date_created_snst;
        [ObservableProperty]
        private ObservableCollection<Bitmap> _photos = new();

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

}
   