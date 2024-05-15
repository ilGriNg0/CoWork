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
        private readonly ObservableCollection<ListItemTemplate> items =
        [
           new ListItemTemplate(typeof(CardViewModel)),
           new ListItemTemplate(typeof(LoginViewModel)),
        ];

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

    public class ListItemTemplate(Type type)
    {
        public object Instance { get; } = Activator.CreateInstance(type);
        public string Label { get; } = type.Name.Replace("ViewModel", "");
        public Type ModelType { get; } = type;
    }
}