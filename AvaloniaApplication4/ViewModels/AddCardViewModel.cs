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
using ReactiveUI;
using Tmds.DBus.Protocol;
using System.Collections;
using System.Net.WebSockets;
namespace AvaloniaApplication4.ViewModels
{
    public partial class AddCardViewModel : ViewModelBase
    {
        [ObservableProperty]
        private JsonClass _json = new();

        //[ObservableProperty]
        //private TariffElements _tariffElementss = new();

        [ObservableProperty]
        private string _errorMessage;
        
        [ObservableProperty]
        private string? _path_ph;
        
        [ObservableProperty]
        private bool _visible  = false;
        
        [ObservableProperty]
        private ObservableCollection<Bitmap> _photo_Coworking = new();
        
        [ObservableProperty]
        private ObservableCollection<Bitmap> _photo_Services = new();

        [ObservableProperty]
         private ObservableCollection<Benefits> _benefitsList = new();

        //[ObservableProperty]
        //private ObservableCollection<TextBoxElements> _textBoxes = new();

        [ObservableProperty]
        private ObservableCollection<JsonClass>_borders = new();

        [ObservableProperty]
        private List<JsonClass> _servicesList = new();

        [ObservableProperty]
        ObservableCollection<string> _strings = new ObservableCollection<string>();

        [ObservableProperty]
        private Dictionary<int, ObservableCollection<JsonClass>> pairs = new();

        [ObservableProperty]
        private static int _countCollection;

        [ObservableProperty]
        private Bitmap? ph1;
        [ObservableProperty]
        private Bitmap? ph2;
        [ObservableProperty]
        private Bitmap? ph3;
        [ObservableProperty]
        private Bitmap? ph4;
        [ObservableProperty]
        private Bitmap? ph5;
        [ObservableProperty]
        private Bitmap? phtest;

        public ObservableCollection<JsonClass> jsonClasses { get; set; } = new ObservableCollection<JsonClass>();
        public ICommand SaveCommand => new RelayCommand(add_collect);

        public ICommand AddPhotoCommand => new RelayCommand<string>(add_photo);
        public ICommand AddPhotoServicesCommand => new RelayCommand(add_photoServices);

        //public ICommand ChoiceServicesCommand => new RelayCommand(ChoiceServices);

        public ICommand ChoiceBenefitsCommand => new RelayCommand<string>(AddYourBenefits);

        public ICommand AddTextBoxesCommand => new RelayCommand(AddTextBox);

        public ICommand DeleteTextBoxesCommand => new RelayCommand(DeleteTextBox);
        public AddCardViewModel()
        {

            Photo_Coworking =
   [
    ph1 = new Bitmap("C:\\Users\\igora\\source\\repos\\CoWork\\апр\\AvaloniaApplication4\\Assets\\pict1.jpg"),
    ph2 = new Bitmap("C:\\Users\\igora\\source\\repos\\CoWork\\апр\\AvaloniaApplication4\\Assets\\pict1.jpg"),
    ph3 = new Bitmap("C:\\Users\\igora\\source\\repos\\CoWork\\апр\\AvaloniaApplication4\\Assets\\pict1.jpg"),
    ph4 = new Bitmap("C:\\Users\\igora\\source\\repos\\CoWork\\апр\\AvaloniaApplication4\\Assets\\pict1.jpg"),
    ph5 = new Bitmap("C:\\Users\\igora\\source\\repos\\CoWork\\апр\\AvaloniaApplication4\\Assets\\pict1.jpg"),
   ];
        }
        ConnectingBD connect = new();
      
       
        public async void add_collect()
        {
            var card = CardViewModel.Instance;

            //CardViewModel card = new();
            var businesse = BusinessAccountViewModel.Instance;
            string price_day = $"От {Json.Price_day_cowork} руб/день" ;
            string price_meeting = $"От {Json.Price_meetingroom_cowork} руб/час";
           
            CountCollection = card.Card_Collection.Count+1;

            string test = IsValidTime(Json.Date_created);
            var item = new JsonClass { Id = CountCollection,
                Info_cowork = Json.Info_cowork,
                Location_cowork = Json.Location_cowork,
                Name_cowork = Json.Name_cowork,
                Price_day_cowork = price_day,
                Price_meetingroom_cowork = price_meeting,
                Date_created_snst = test,
                Date_created = test,
                Id_busin = BusinessAccountViewModel.IdBusinUser,
                
                 };
           
            //if (!IsValidTime(Json.Date_created))
            //{
            //    Json.Date_created = null;
            //}

            if (item.Info_cowork != null && item.Location_cowork != null
                && item.Name_cowork != null  && item?.Date_created != null 
                && Photo_Coworking.Count == 5 && Borders.Count != 0)
            {
                jsonClasses.Add(item);
                //Pairs.Add(BusinessAccountViewModel.IdBusinUser, jsonClasses);
                card.Card_Collection.Add(jsonClasses);
                //foreach (var itm in card.Card_Collection)
                //{
                //    if (!businesse.BookingsBusines.ContainsKey(itm.Id))
                //    {
                //        businesse.BookingsBusines[BusinessAccountViewModel.IdBusinUser] = new ObservableCollection<JsonClass>();
                //    }

                //    // Check if the item is already in the collection
                //    if (!businesse.BookingsBusines[BusinessAccountViewModel.IdBusinUser].Any(x => x.Id == itm.Id))
                //    {
                //        businesse.BookingsBusines[BusinessAccountViewModel.IdBusinUser].Add(itm);
                //    }
                //}
                connect?.WriteNormalBD(card.Card_Collection.Count, item.Info_cowork, item.Date_created, item.Date_created_snst, item.Name_cowork, item.Location_cowork, item.Raiting_count, item.Rating_sum, BusinessAccountViewModel.IdBusinUser);
                businesse.InsertBookings();
                await connect.WriteBusinessBd(Strings, card.Card_Collection.Count);
                connect.WriteBenefitsBd(BenefitsList);
                connect.WriteServicesBd(card.Card_Collection.Count, Borders, Strings);
              
                var main = MainWindowViewModel.Instance;
                //main.Navigate("BusinessAccountViewModel");
                main.Page = (ViewModelBase)Activator.CreateInstance(typeof(BusinessAccountViewModel));

            }
            else
            {
                ErrorMessage = "Заполните поля или добавьте фото";
            }

        }
        public string IsValidTime(string timeRange)
        {

            var parts = timeRange.Split(new[] { " -- " }, StringSplitOptions.None);

           
            if (parts.Length != 2)
            {
                return timeRange = null;
            }

          
            if (TimeSpan.TryParse(parts[0], out TimeSpan startTime) && TimeSpan.TryParse(parts[1], out TimeSpan endTime))
            {
               
                if (startTime < endTime)
                {
                    return timeRange;
                }
            }

           
            return timeRange = null;
        }
        public async void AddYourBenefits(string info)
        {

            var card = CardViewModel.Instance;
            var BenefitsDict = new Dictionary<string, string>
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
            if (!BenefitsList.Any(b => b.Content == info))
            {
                if (BenefitsDict.TryGetValue(info, out var icon))
                {
                    var benefit = new Benefits
                    {
                        Id_Coworking = card.Card_Collection.Count+1,
                        Content = info,
                        Icon = icon
                    };

                    BenefitsList.Add(benefit);
                }
            }
            
            
            

        }
        //public  void ChoiceServices()
        //{

        //    foreach (var item in Borders)
        //    {
        //        //if (!ServicesList.Any(x => x.Tariffs == item.Tarif && x.Tariffs_price == item.Tarif_price))
        //        //{
        //        //    ServicesList.Add(new JsonClass { Tariffs = item.Tarif, Tariffs_price = item.Tarif_price, Number_of_seats_solo = item.Tarif_count, Pserv = item.Tarif_ph });
        //        //}
        //    }


        //    //connect?.WriteServicesBd(cnt = card.Card_Collection.Count + 1, price1, count1, tasl1, tasl2, "Assets\\context.png", price2, count2);
        //}
        private readonly Window _target = new();
        public static FilePickerFileType ImageAll { get; } = new("All Images")
        {
            Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.gif", "*.bmp", "*.webp" },
            AppleUniformTypeIdentifiers = new[] { "public.image" },
            MimeTypes = new[] { "image/*" }
        };

        public async void add_photo(string cnt) 
        {
           
          
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                FileTypeFilter = new[] { ImageAll, FilePickerFileTypes.TextPlain }
            });
            if (files != null && files.Count >= 1 && Photo_Coworking.Count <= 5)
            {
                var selectedFile = files[0];
                var filePath = selectedFile.Path.LocalPath;
                string filepath = "Assets\\" + Path.GetFileName(filePath);
                if (!File.Exists(filepath))
                {
                    File.Copy(filePath, filepath);
                }
                
                Strings.Add(filepath);
                //Photo_Coworking[ind] = new Bitmap(filePath);
              


                if (int.TryParse(cnt, out int index))
                {
                    if (index >= 0 && index < Photo_Coworking.Count)
                    {
                      
                        Photo_Coworking[index] = new Bitmap(filepath);
                       
                    }
                    else if (index >= 0 && Photo_Coworking.Count < 5)
                    {
                        Photo_Coworking.Add(new Bitmap(filepath));
                    }
                }


            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
               
        }
        public async void add_photoServices()
        {
            

            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                FileTypeFilter = new[] { ImageAll, FilePickerFileTypes.TextPlain }
            });
            if (files != null)
            {
                var selectedFile = files[0];
                var filePath = selectedFile.Path.LocalPath;
                string filepath = "Assets\\" + Path.GetFileName(filePath);
                Path_ph = filepath;
                Json.Pserv = filepath;
                if (!File.Exists(filepath))
                {
                    File.Copy(filePath, filepath);
                }
                Photo_Services.Add(new Bitmap(filePath));
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
        public void AddTextBox()
        {
            Borders.Add(new JsonClass());
            Visible = true;
        }

        public void DeleteTextBox()
        {
            if (Borders.Count >= 1 && ServicesList.Count >= 1)
            {  
                Borders.RemoveAt(Borders.Count - 1);
                ServicesList.RemoveAt(ServicesList.Count - 1);

            }
            
            Visible = false;

        }
    }
    

}
