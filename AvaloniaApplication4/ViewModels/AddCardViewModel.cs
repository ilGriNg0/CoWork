using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Controls.Platform;
using AvaloniaApplication4.Views;
namespace AvaloniaApplication4.ViewModels
{
    public partial class AddCardViewModel : ViewModelBase
    {
        [ObservableProperty]
        private JsonClass _json = new();

        [ObservableProperty]
        private string _errorMessage;
        [ObservableProperty]
        private string? _path_ph;
        [ObservableProperty]
        private ObservableCollection<Bitmap> _photo_Coworking = new();

        [ObservableProperty]
         private ObservableCollection<Benefits> _benefitsList = new();
        public ObservableCollection<JsonClass> jsonClasses { get; set; } = new ObservableCollection<JsonClass>();
        public ICommand SaveCommand => new RelayCommand(add_collect);

        public ICommand AddPhotoCommand => new RelayCommand(add_photo);

        public ICommand ChoiceServicesCommand => new RelayCommand<string>(ChoiceServices);
        
        public AddCardViewModel() { }
        ConnectingBD connect = new();
        public async void add_collect()
        {
          
            CardViewModel card = new();
            BusinessAccountViewModel businesse = new();
            string price_day = $"От {Json.Price_day_cowork} руб/день" ;
            string price_meeting = $"От {Json.Price_meetingroom_cowork} руб/час";
           
            int counter = card.Card_Collection.Count+1;
            int cnt = 0;
            foreach (var items in AddCardView.strings)
            {
                Json?.JsonBenefits.Add(items);
                
            }
            AddCardView.strings = new();
            
            var item = new JsonClass { Id = counter,
                Info_cowork = Json.Info_cowork,
                Location_cowork = Json.Location_cowork,
                Name_cowork = Json.Name_cowork,
                Price_day_cowork = price_day,
                Price_meetingroom_cowork = price_meeting,
                Date_created_snst = Json.Date_created_snst,
                Date_created = Json.Date_created,
                Number_of_seats_solo = Json.Number_of_seats_solo,
                Number_of_seats_meeting = Json.Number_of_seats_meeting,
                 };

            if (item.Info_cowork != null && item.Location_cowork != null
                && item.Name_cowork != null && item.Price_day_cowork != null
                && item.Price_meetingroom_cowork != null && item?.Date_created_dt != null 
                && item?.Date_created_snst_dt != null && Photo_Coworking.Count == 5)
            {
                jsonClasses.Add(item);
                connect?.WriteNormalBD(item.Id, item.Info_cowork, item.Date_created, item.Date_created_snst, item.Name_cowork, item.Location_cowork, item.Raiting_count, item.Rating_sum);
                card.Card_Collection.Add(jsonClasses);
                businesse.BookingsBusines.Add(jsonClasses);
                businesse.InsertBookings();
                
            }
            else
            {
                ErrorMessage = "Заполните поля или добавьте фото";
            }

        }
        public  async void ChoiceServices(string collect)
        {
           CardViewModel card = new CardViewModel();
            int cnt;
            var price1 = int.TryParse(Json.Price_day_cowork, out var parsedPrice1) ? parsedPrice1 : 0;
            var price2 = int.TryParse(Json.Price_meetingroom_cowork, out var parsedPrice2) ? parsedPrice2 : 0;
            var count1 = Json.Number_of_seats_solo;
            var count2 = Json.Number_of_seats_meeting;
            string tasl1 = "Рабочее место";
            string tasl2 = "Переговорная";
            var servicesDict = new Dictionary<string, string>
                {
                    { "Высокоскоростной интернет", "Wifi" },
                    { "Качественный кофе", "CoffeeOutline" },
                    { "Принтер", "PrinterOutline" },
                    { "Шкафчики для вещей", "Locker" },
                    { "Душевые", "ShowerHead" },
                    { "Фрукты и закуски", "FoodAppleOutline" },
                    { "Столовая", "SilverwareForkKnife" },
                    { "Парковка", "Parking" },
                    { "Фитнес зона", "Dumbbell" }
                };

                //add_photo();
           
                connect?.WriteServicesBd(cnt = card.Card_Collection.Count + 1, price1, count1, tasl1, tasl2, "Assets\\context.png", price2, count2);
            

         

            if (servicesDict.TryGetValue(collect, out var icon))
            {
                var benefit = new Benefits
                {
                    Id_Coworking = card.Card_Collection.Count + 1,
                    Content = collect,
                    Icon = icon
                };
                BenefitsList.Add(benefit);
            }
            connect.WriteBenefitsBd(BenefitsList);
        }
        private readonly Window _target = new();
        public static FilePickerFileType ImageAll { get; } = new("All Images")
        {
            Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.gif", "*.bmp", "*.webp" },
            AppleUniformTypeIdentifiers = new[] { "public.image" },
            MimeTypes = new[] { "image/*" }
        };
        public async void add_photo() 
        {
            CardViewModel cardView = new();
          
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                FileTypeFilter = new[] { ImageAll, FilePickerFileTypes.TextPlain }
            });
            if (files != null && files.Count >= 1 && Photo_Coworking.Count <= 5)
            {
                var selectedFile = files[0];
                var filePath = selectedFile.Path.LocalPath;
                string filepath = "Assets\\" + Path.GetFileName(filePath);
                Path_ph = filepath;
                if (!File.Exists(filepath))
                {
                    File.Copy(filePath, filepath);
                }
               await connect.WriteBusinessBd(filepath, cardView.Card_Collection.Count + 1);
                Photo_Coworking.Add(new Bitmap(filePath));
             
            }
               
        }
        
    }
    
}
