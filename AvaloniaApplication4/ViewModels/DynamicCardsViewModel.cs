using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvaloniaApplication4.ViewModels.PersonalAccountViewModel;

namespace AvaloniaApplication4.ViewModels
{
    public partial class DynamicCardsViewModel:ViewModelBase
    {

        [ObservableProperty]
        private ObservableCollection<JsonClass> _collection = new();

        

       public DynamicCardsViewModel(Booking bk)
        {
            LoadData(bk);
        }
        public DynamicCardsViewModel(JsonClass item)
        {
            LoadData(item);
        }
        public void LoadData(JsonClass item)
        {
            CardViewModel c_viewModel = new CardViewModel();
            var items = c_viewModel.Card_Collection.FirstOrDefault(p => p.Id == item.Id);
            if (items != null)
            {
                Collection.Add(items);
            }
        }
        public void LoadData(Booking item)
        {
            CardViewModel c_viewModel = new CardViewModel();
            var items = c_viewModel.Card_Collection.FirstOrDefault(p => p.Id == item.id_coworking);
            if (items != null)
            {
                Collection.Add(items);
            }
        }
    }
}
