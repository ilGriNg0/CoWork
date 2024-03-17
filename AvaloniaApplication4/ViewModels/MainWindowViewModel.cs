using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Reactive;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using Avalonia.Skia.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using AvaloniaApplication4.Models;
namespace AvaloniaApplication4.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        ////public Bitmap? Image_bitmap_source { get; } = ImageLoad.Load("/Assets/prosto.jpg");
        
        [ObservableProperty]
        private ViewModelBase _page = new CardViewModel();

       
        [ObservableProperty] private CardModel? _cardOpen;
      
        public ObservableCollection<CardModel> cards { get; } = new()
        {
           new CardModel(typeof(CardViewModel))
        };
    }
}
//< Image Grid.ColumnSpan = "2"
//Grid.RowSpan = "2"
//Source = "{Binding Image_bitmap_source}"
//Stretch = "Fill" ></ Image >