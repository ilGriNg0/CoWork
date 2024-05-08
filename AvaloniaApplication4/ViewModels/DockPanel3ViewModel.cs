using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
namespace AvaloniaApplication4.ViewModels
{
    public partial class DockPanel3ViewModel : ViewModelBase
    {
        ConnectingBD connecting = new ConnectingBD();
        [ObservableProperty]
        private Dictionary<int, List<JsonClass>> _data;

        [ObservableProperty]
        private List<JsonClass> _peopleCollection = new();

        [ObservableProperty]
        private List<JsonClass> _border3 = new();
        public DockPanel3ViewModel()
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
                    case 3:
                        Border3.Add(item);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
