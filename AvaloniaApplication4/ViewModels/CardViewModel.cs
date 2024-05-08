
using Avalonia.Controls.Documents;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;

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

    ConnectingBD connecting = new();

    [ObservableProperty]
    private Dictionary<int, List<JsonClass>> _data;

    [ObservableProperty]
    private List<JsonClass> _peopleCollection = new();
    
    [ObservableProperty]
    private List<JsonClass> _border1 = new();
    
    [ObservableProperty]
    private List<JsonClass> _border2 = new();

    [ObservableProperty]
    private List<JsonClass> _border3 = new();
    public CardViewModel()
    {
        connecting.ReadBd();
        OnDataLoad();
    }
    public void OnDataLoad()
    {
        Data = connecting.keyValuePairs;


        foreach (var item in Data)
        {
            PeopleCollection.AddRange(item.Value.Where(p => p.Id > 0 && p.Name_cowork != string.Empty && p.Info_cowork != string.Empty && p.Location_metro_cowork != string.Empty && p.Price_day_cowork != string.Empty && p.Price_meetingroom_cowork != string.Empty));
            //var Items = item.Value.Where(p => p.Id > 0 && p.Name != string.Empty);
            //foreach (var collect in Items)
            //{
            //    classes.Add(collect);
            //}
        }
        add();

    }
    public void add()
    {

        foreach (var item in PeopleCollection)
        {
            switch (item?.Id)
            {
                case 1:
                    Border1.Add(item);
                    break;
                case 2:
                    Border2.Add(item);
                    break;
                case 3:
                    Border3.Add(item);
                    break;
                default:
                    break;
            }
        }
    }
}