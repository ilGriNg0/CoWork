
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media.Imaging;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AvaloniaApplication4.ViewModels;

public partial class CardViewModel : ViewModelBase
{
 
    [ObservableProperty]
    private ViewModelBase _card_add = new AddCardViewModel();
    
    ConnectingBD connecting = new();

    [ObservableProperty]
    private Dictionary<int, List<JsonClass>> _data;

    [ObservableProperty]
    private List<JsonClass> _peopleCollection = new();

    [ObservableProperty]
    private ObservableCollection<JsonClass> _card_Collection = new();


    public ICommand NavigateCommand => new RelayCommand<string>(Navigate);

    public string Namespace()
    {
        Type tp = typeof(CardViewModel);
        string? yNameSpc = tp.Namespace;
        return yNameSpc;
    }

    public CardViewModel()
     {
        connecting.ReadNormalBD();
        connecting.ReadServicesBd();
        connecting.ReadPhotoBd();
        OnDataLoad();
    }
    private static CardViewModel? _instance;
    public static CardViewModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CardViewModel();
            }
            return _instance;
        }


    }
    public void Navigate(string? pageViewModel)
    {
        var main = MainWindowViewModel.Instance;
        main.Navigate(pageViewModel);
    }
    public void OnDataLoad()
    {
        Data = connecting.keyValuePairs;
        foreach (var item in Data)
        {
            var Items = item.Value.Where(p => p.Name_cowork != string.Empty && p.Info_cowork != string.Empty && p.Location_cowork != string.Empty && p.Price_day_cowork != string.Empty && p.Price_meetingroom_cowork != string.Empty);
            foreach (var collect in Items)
            {
                Card_Collection.Add(collect);

            }

        }
        foreach (var item in connecting.ServicesPairs)
        {
            foreach (var itemz in Card_Collection)
            {
                if (item.Value.Item1 == itemz.Id && itemz.Price_day_cowork == null) 
                {
                    itemz.Price_day_cowork = $"От {item.Value.Item2.ToString()} руб/день";
                
                }
                if (item.Key != itemz.Id)
                {
                    itemz.Price_meetingroom_cowork = $"От {item.Value.Item2.ToString()} руб/час";
                }
            }
        }
        foreach (var item in connecting.PhotoIDPathPairs)
        {
            foreach (var item2 in Card_Collection)
            {
                if(item.Key.Item2 == item2.Id)
                {
                    item2.Photos.Add(new Bitmap(item.Value));
                }
            }
        }

    }
    
}