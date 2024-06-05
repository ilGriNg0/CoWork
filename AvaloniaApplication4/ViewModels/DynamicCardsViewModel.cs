using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static AvaloniaApplication4.ViewModels.PersonalAccountViewModel;

namespace AvaloniaApplication4.ViewModels
{
    public partial class DynamicCardsViewModel : ViewModelBase
    {

        [ObservableProperty]
        private ObservableCollection<JsonClass> _collection = new();

        [ObservableProperty]
        private ObservableCollection<Bitmap> _photoses = new();

        [ObservableProperty]
        private ObservableCollection<TariffElements>  _tariffElem = new();  

        [ObservableProperty]
        private static bool _isreguser = false;
        //[ObservableProperty]
        //private ObservableCollection<mainservs> _serv = new();
        [ObservableProperty]
        private ObservableCollection<Benefits> _benef = new();
        [ObservableProperty]
       private List<Benefits> _ben = new List<Benefits>();

        [ObservableProperty]
        private string? _content;

        [ObservableProperty]
        private string? _icons;

        [ObservableProperty]
        private string? _message;

        [ObservableProperty]
        private bool _isOn = true;

        public ICommand NavigateCommand => new RelayCommand<string>(Navigate);
        public string Namespace()
        {
            Type tp = typeof(MainWindowViewModel);
            string? yNameSpc = tp.Namespace;
            return yNameSpc;
        }
        public void Navigate(string? pageViewModel)
        {
            var instance = (LoginViewModel)Activator.CreateInstance(typeof(LoginViewModel), true);
            var instanceBs = (BusinessAccountViewModel)Activator.CreateInstance(typeof(BusinessAccountViewModel), true);
            bool reg = instance.IsReg;
            var main = MainWindowViewModel.Instance;
            if (reg)
            {
                main.Page = (ViewModelBase)Activator.CreateInstance(Type.GetType(Namespace() + "." + pageViewModel)); 


            }
            else if(instanceBs.KeyBusin)
            {
                IsOn = false;
                Message = "Оформление бронирования невозможно. Вы зарегистрированы как бизнес-аккаунт";
            }
            else
            {
                main.Page = (ViewModelBase)Activator.CreateInstance(typeof(LoginViewModel));
            }
            
          
        }
        public DynamicCardsViewModel() { }
        public DynamicCardsViewModel(Booking bk)
        {
            LoadData(bk);
        }
        public DynamicCardsViewModel(JsonClass item)
        {
            LoadData(item);

        }
        public void LoadData<T>(T item)
        {
            var instance = (CardViewModel)Activator.CreateInstance(typeof(CardViewModel), true);
            ConnectingBD connecting = new();

            if (item is JsonClass js)
            {
                var items = instance?.Card_Collection.FirstOrDefault(p => p.Id == js.Id && p.Name_cowork == js.Name_cowork);
                connecting.ReadServicesBd(js);
                JsonClass jsonClass = DynamicCardsViewModel.BackupDatajs;
               
                if (js.Equals(jsonClass))
                {
                    jsonClass = js;
                    Collection.Add(jsonClass);
                    Photoses.Add(js.Photos);
                    connecting.ReadBenefitsBd(js);

                    foreach (var itemz in connecting.keyValueBenefitsPairs)
                    {
                        foreach (var itemsdf in itemz.Value)
                        {
                            Benef.Add(itemsdf);
                        }
                    }
                    int i = 0;
                    foreach (var item_serv in connecting.ServicesPairs)
                    {
                        TariffElem.Add(new TariffElements { Tarif = item_serv.Value.Item4, Tarif_count = item_serv.Value.Item3, Tarif_price = item_serv.Value.Item2 , Tarif_id = i++,});
                    }
                }
                else
                {
                    if (items != null)
                    {
                        Collection.Add(items);
                    }
                    foreach (var item_ph in js.Photos)
                    {
                        Photoses.Add(item_ph);

                    }
                    //js.Photos.Clear();
                    connecting.ReadBenefitsBd(js);

                    foreach (var itemz in connecting.keyValueBenefitsPairs)
                    {
                        foreach (var itemsdf in itemz.Value)
                        {
                            Benef.Add(itemsdf);
                        }
                    }

                    int i = 0;
                    foreach (var item_serv in connecting.ServicesPairs)
                    {
                        TariffElem.Add(new TariffElements { Tarif = item_serv.Value.Item4, Tarif_count = item_serv.Value.Item3, Tarif_price = item_serv.Value.Item2, Tarif_id = i++ });
                    }
                }
               
               
                SaveDatajs(js);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            if (item is Booking book)
            {
                Booking bookClass = DynamicCardsViewModel.BacupDatabk;
                var items = instance?.Card_Collection.FirstOrDefault(p => p.Id == book.id_coworking && p.Name_cowork == book.Name_Cowork);

                if (item.Equals(bookClass))
                {

                    book = bookClass;
                    Collection.Add(items);
                    Photoses.Add(book.B_maps);
                    connecting.ReadBenefitsBd(book);
                    foreach (var itemz in connecting.keyValueBenefitsPairs)
                    {
                        foreach (var itemsdf in itemz.Value)
                        {
                            Benef.Add(itemsdf);
                        }
                    }
                    int i = 0;
                    foreach (var item_serv in connecting.ServicesPairs)
                    {
                        TariffElem.Add(new TariffElements { Tarif = item_serv.Value.Item4, Tarif_count = item_serv.Value.Item3, Tarif_price = item_serv.Value.Item2, Tarif_id = i++ });
                    }
                }
                else
                {
                    //var items = instance?.Card_Collection.FirstOrDefault(p => p.Id == book.id_coworking);
                    connecting.ReadServicesBd(book);

                    if (items != null)
                    {
                        Collection.Add(items);
                    }
                    foreach (var item_book in book.B_maps)
                    {
                        Photoses.Add(item_book);
                    }
                    connecting.ReadBenefitsBd(book);

                    foreach (var itemz in connecting.keyValueBenefitsPairs)
                    {
                        foreach (var itemsdf in itemz.Value)
                        {
                            Benef.Add(itemsdf);
                        }
                    }
                    int i = 0;
                    foreach (var item_serv in connecting.ServicesPairs)
                    {
                        TariffElem.Add(new TariffElements { Tarif = item_serv.Value.Item4, Tarif_count = item_serv.Value.Item3, Tarif_price = item_serv.Value.Item2 , Tarif_id = i++ });
                    }
                }

                SaveDatabk(book);
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //}
            }

            }
    

        private static JsonClass? _backupDatajs;

        public static JsonClass BackupDatajs
        {
            get => _backupDatajs;
            set => _backupDatajs = value;
        }

        public void SaveDatajs(JsonClass data_js) => BackupDatajs = data_js;


        private static Booking? _bacupDatabk;

        public static Booking BacupDatabk
        {
            get => _bacupDatabk;
            set => _bacupDatabk = value;
        }

        public void SaveDatabk(Booking data_bk) => BacupDatabk = data_bk;
    }
    public partial class Benefits : ObservableObject
    {
        [ObservableProperty]
        private int _id_Coworking;

        [ObservableProperty]
        private string? _content;

        [ObservableProperty]
        private string? _icon;
    }

}

