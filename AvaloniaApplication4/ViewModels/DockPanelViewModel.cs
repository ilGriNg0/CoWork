using Avalonia.Controls;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaApplication4.ViewModels;

public partial class DockPanelViewModel : ViewModelBase
{
    ConnectingBD connecting = new ConnectingBD();
    [ObservableProperty]
    private Dictionary<int, List<JsonClass>> _data;

    [ObservableProperty]
    private List<JsonClass> _peopleCollection = new();

    [ObservableProperty]
    private List<JsonClass> _border1 = new();
    public DockPanelViewModel()
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
                default:
                    break;
            }
        }
    }
}