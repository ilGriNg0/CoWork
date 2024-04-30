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
        private readonly ObservableCollection<ListItemTemplate> items =
        [
           new ListItemTemplate(typeof(CardViewModel)),
           new ListItemTemplate(typeof(LoginViewModel)),
           new ListItemTemplate(typeof(RegistrationViewModel)),
           new ListItemTemplate(typeof(BusinessAccountViewModel)),
           new ListItemTemplate(typeof(PersonalAccountViewModel))
        ];
        [ObservableProperty]
        private ViewModelBase _currentPage = new CardViewModel();

       
        [ObservableProperty] 
        private ListItemTemplate? _selectedListItem;

        public ObservableCollection<ListItemTemplate> Items => items;
        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null) return;
            CurrentPage = (ViewModelBase)value.Instance;
        }
    }

    public class ListItemTemplate(Type type)
    {
        public object Instance { get; } = Activator.CreateInstance(type);
        public string Label { get; } = type.Name.Replace("ViewModel", "");
        public Type ModelType { get; } = type;
    }
}