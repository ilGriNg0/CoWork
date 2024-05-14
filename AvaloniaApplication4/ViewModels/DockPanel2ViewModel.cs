using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using AvaloniaApplication4.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Tmds.DBus.Protocol;
using CommunityToolkit.Mvvm.ComponentModel;
namespace AvaloniaApplication4.ViewModels
{
    public partial class DockPanel2ViewModel : ViewModelBase
    {
        ConnectingBD connecting = new();

        [ObservableProperty]
        private Dictionary<int, List<JsonClass>> _data;

        [ObservableProperty]
        private List<JsonClass> _peopleCollection = new();

        [ObservableProperty]
        private List<JsonClass> _border2 = new();
        public DockPanel2ViewModel()
        {
            connecting.ReadBd();
            OnDataLoad();
        }
        public void OnDataLoad()
        {
            Data = connecting.keyValuePairs;


            foreach (var item in Data)
            {
                PeopleCollection.AddRange(item.Value.Where(p => p.Id==2));
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
                    case 2:
                        Border2.Add(item);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
