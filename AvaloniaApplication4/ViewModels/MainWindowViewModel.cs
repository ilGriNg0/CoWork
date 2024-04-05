using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using Avalonia.Skia.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using Npgsql;
using ReactiveUI;
namespace AvaloniaApplication4.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        
        [ObservableProperty]
        private ViewModelBase _currentPage = new CardViewModel();

       
        [ObservableProperty] 
        private ListItemTemplate? _selectedListItem;
      
        public ObservableCollection<ListItemTemplate> Items { get; } = new()
        {
           new ListItemTemplate(typeof(CardViewModel)),
           new ListItemTemplate(typeof(LogOrRegViewModel)) 
        };

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null) return;
            var instance = Activator.CreateInstance(value.ModelType);
            if (instance is null) return;
            CurrentPage = (ViewModelBase)instance;
        }
    }

    public class ListItemTemplate
    {
        public ListItemTemplate(Type type)
        {
            ModelType = type;
            Label = type.Name.Replace("ViewModel", "");
        }

        public string Label { get; }
        public Type ModelType { get; }
    }
}