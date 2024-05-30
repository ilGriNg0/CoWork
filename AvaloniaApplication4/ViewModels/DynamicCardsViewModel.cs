using Avalonia.Controls;
using Avalonia.Media.Imaging;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvaloniaApplication4.ViewModels.PersonalAccountViewModel;

namespace AvaloniaApplication4.ViewModels
{
    public partial class DynamicCardsViewModel : ViewModelBase
    {

        [ObservableProperty]
        private ObservableCollection<JsonClass> _collection = new();

        [ObservableProperty]
        private ObservableCollection<Bitmap> _photoses = new();

       
        //[ObservableProperty]
        //private ObservableCollection<mainservs> _serv = new();
        [ObservableProperty]
        private ObservableCollection<Benefits> _benef = new();

        [ObservableProperty]
        private string? _content;

        [ObservableProperty]
        private string? _icons;
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
            CardViewModel c_viewModel = new CardViewModel();
            ConnectingBD connecting = new();
            List<Benefits> ben = new List<Benefits>();
            if (item is JsonClass js)
            {
                var items = c_viewModel.Card_Collection.FirstOrDefault(p => p.Id == js.Id);
                connecting.ReadServicesBd();
                if (items != null)
                {
                    Collection.Add(items);
                }
                foreach (var item_ph in js.Photos)
                {
                    Photoses.Add(item_ph);
                }
                //foreach (var item3 in connecting.ServicesPairs)
                //{
                //    if (item3.Value.Item1 == js.Id && item3.Value.Item4 != string.Empty)
                //    {

                //        Serv.Add(new mainservs {Value1 = item3.Value.Item4, Value2 = item3.Value.Item5}); 
                //    }
                connecting.ReadBenefitsBd();
                Benef = new ObservableCollection<Benefits>(
                     connecting.keyValueBenefitsPairs
                    .SelectMany(kvp => kvp.Value)
                        .Select(benefit => new Benefits
                    {
                         Content = benefit.Content,
                         Icon = benefit.Icon
                        })
                    );




            }
                if (item is Booking book)
                {
                    var items = c_viewModel.Card_Collection.FirstOrDefault(p => p.Id == book.id_coworking);
                    connecting.ReadServicesBd();
                    if (items != null)
                    {
                        Collection.Add(items);
                    }
                    foreach (var item_book in book.B_maps)
                    {
                        Photoses.Add(item_book);
                    }
                connecting.ReadBenefitsBd();
                Benef = new ObservableCollection<Benefits>(
                      connecting.keyValueBenefitsPairs
                     .SelectMany(kvp => kvp.Value)
                         .Select(benefit => new Benefits
                         {
                             Content = benefit.Content,
                             Icon = benefit.Icon
                         })
                     );
                //    foreach (var item3 in connecting.ServicesPairs)
                //    {
                //    if (item3.Value.Item1 == book.id_coworking && item3.Value.Item4 != string.Empty)
                //    {

                //        Serv.Add(new mainservs { Value1 = item3.Value.Item4, Value2 = item3.Value.Item5 });
                //    }


                //}
            }


            }
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
    public partial class mainservs : ObservableObject
    {
        [ObservableProperty]
        private int _key1;
        [ObservableProperty]
        private string? _value1;
        [ObservableProperty]
        private string? _value2;

       
    }
}

