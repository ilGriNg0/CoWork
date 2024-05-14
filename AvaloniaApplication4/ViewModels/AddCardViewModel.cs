using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
namespace AvaloniaApplication4.ViewModels
{
    public partial class AddCardViewModel : ViewModelBase
    {
        [ObservableProperty]
        private JsonClass _json = new();

        [ObservableProperty]
        private string _errorMessage;

        public ObservableCollection<JsonClass> jsonClasses { get; set; } = new ObservableCollection<JsonClass>();
        public ICommand SaveCommand => new RelayCommand(add_collect);

        public AddCardViewModel() { }
        public void add_collect()
        {
            ConnectingBD connect = new();
            var item = new JsonClass { Id = Json.Id, Info_cowork = Json.Info_cowork, Location_metro_cowork = Json.Location_metro_cowork, Name_cowork = Json.Name_cowork, Price_day_cowork = Json.Price_day_cowork, Price_meetingroom_cowork = Json.Price_meetingroom_cowork};
            if(item.Info_cowork != null && item.Location_metro_cowork != null && item.Name_cowork != null && item.Price_day_cowork != null && item.Price_meetingroom_cowork != null  )
            {
                jsonClasses.Add(item);
                //connect?.WriteBd(jsonClasses);
            }
            else 
            {
                ErrorMessage = "Заполните поля";
            }
           
        }
    }
}
