
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
    //[ObservableProperty]
    //private ViewModelBase _open = new CardOpenViewModel();
    [ObservableProperty]
    private ViewModelBase _base = new CardOpenViewModel();
    [ObservableProperty]
    private ViewModelBase _base2_card = new Card1ViewModel();
    [ObservableProperty]
    private ViewModelBase _base3_card = new Card2ViewModel();

    

    [ObservableProperty]
    private ViewModelBase _card_add = new AddCardViewModel();
    
    ConnectingBD connecting = new();

    [ObservableProperty]
    private Dictionary<int, List<JsonClass>> _data;

    [ObservableProperty]
    private List<JsonClass> _peopleCollection = new();

    [ObservableProperty]
    private ObservableCollection<JsonClass> _card_Collection = new();

    //public ObservableCollection<JsonClass> Companies { get; set; } = new();
    [ObservableProperty]
    private List<JsonClass> _border1 = new();
    
    [ObservableProperty]
    private List<JsonClass> _border2 = new();

    [ObservableProperty]
    private List<JsonClass> _border3 = new();

    public ICommand NavigateCommand => new RelayCommand<string>(Navigate);

    public string Namespace()
    {
        Type tp = typeof(CardViewModel);
        string? yNameSpc = tp.Namespace;
        return yNameSpc;
    }

   
   
 
    public CardViewModel()
    {
        connecting.ReadBd();
        connecting.ReadPhotoBd();
        OnDataLoad();
    }
    public void Navigate(string? pageViewModel)
    {
        var main = MainWindowViewModel.Instance;

        main.Navigate(pageViewModel);
    }
    public void OnDataLoad()
    {
        Data = connecting.keyValuePairs;

        var photosById = new Dictionary<int, List<string>>();

        // Заполняем словарь фотографиями
        foreach (var pair in connecting.PhotoIDPathPairs)
        {
            if (!photosById.ContainsKey(pair.Key.Item2))
            {
                photosById[pair.Key.Item2] = new List<string>();
            }
            photosById[pair.Key.Item2].Add(pair.Value);
        }
        foreach (var item in Data)
        {
            //PeopleCollection.AddRange(item.Value.Where(p => p.Name_cowork != string.Empty && p.Info_cowork != string.Empty && p.Location_cowork != string.Empty && p.Price_day_cowork != string.Empty && p.Price_meetingroom_cowork != string.Empty));
            var Items = item.Value.Where(p => p.Name_cowork != string.Empty && p.Info_cowork != string.Empty && p.Location_cowork != string.Empty && p.Price_day_cowork != string.Empty && p.Price_meetingroom_cowork != string.Empty);
            foreach (var collect in Items)
            {
                Card_Collection.Add(collect);
            }
        }
        // Присваиваем фотографии к людям в коллекции
        foreach (var item in Card_Collection)
        {
            if (photosById.TryGetValue(item.Id, out var photoPaths))
            {
                // Если для данного ID есть несколько фото, выбираем первое, второе и так далее
                for (int i = 0; i < photoPaths.Count; i++)
                {
                    // Здесь мы должны удостовериться, что не выходим за границы коллекции
                    if (i < Card_Collection.Count)
                    {
                        Card_Collection[i].Path_photo = new Bitmap(photoPaths[i]);
                    }
                }
            }
            //foreach (var item in Data)
            //{
            //    PeopleCollection.AddRange(item.Value.Where(p =>  p.Name_cowork != string.Empty && p.Info_cowork != string.Empty && p.Location_metro_cowork != string.Empty && p.Price_day_cowork != string.Empty && p.Price_meetingroom_cowork != string.Empty));
            //    var Items = item.Value.Where(p =>  p.Name_cowork != string.Empty && p.Info_cowork != string.Empty && p.Location_metro_cowork != string.Empty && p.Price_day_cowork != string.Empty && p.Price_meetingroom_cowork != string.Empty);
            //    foreach (var collect in Items)
            //    {
            //        Card_Collection.Add(collect);
            //    }
            //}
            //foreach (var item2 in Card_Collection)
            //{
            //    foreach (var item3 in connecting.PhotoIDPathPairs)
            //    {
            //        if(item2.Id == item3.Key.Item2)
            //        {

            //            item2.Path_photo = new Bitmap(item3.Value);
            //        }

            //    }
            //}
            //add();

        }
    }
    
}