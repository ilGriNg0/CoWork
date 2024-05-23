using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvaloniaApplication4.ViewModels.PersonalAccountViewModel;

namespace AvaloniaApplication4.ViewModels
{
    public partial class CreateBookingViewModel : ViewModelBase
    {
        private int Id_c;
        private int Open;
        private int Closed;
        public CreateBookingViewModel() { }
        public CreateBookingViewModel(int id_с, int open, int closed) 
        {
            Id_c = id_с;
            Open = open;
            Closed = closed;
        }

        private ObservableCollection<string> _types = new ObservableCollection<string>(new List<string>() { "coworking", "place"});
        private string _selectedTyoe;
        public ObservableCollection<string> Types
        {
            get => _types;
            set
            {
                _types = value;
                OnPropertyChanged(nameof(Types));
            }
        }
        public string SelectedType
        {
            get => _selectedTyoe;
            set
            {
                _selectedTyoe = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        private DateTimeOffset _selectedDate = new DateTimeOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
        public DateTimeOffset SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (value < DateTime.Now.Date)
                    _selectedDate = new DateTimeOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                else
                    _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private ObservableCollection<int> _hours = new ObservableCollection<int>(new List<int>() { 1,2,3});
        public ObservableCollection<int> Hours
        {
            get => _hours;
            set
            {
                _hours = value;
                OnPropertyChanged(nameof(Hours));
            }
        }

        private int _selectedHour;
        public int SelectedHour
        {
            get => _selectedHour;
            set
            {
                _selectedHour = value;
                OnPropertyChanged(nameof(SelectedHour));
            }
        }
        private ObservableCollection<int> _times = new ObservableCollection<int>(new List<int>() { 1,2,3,4,5});
        public ObservableCollection<int> Times
        {
            get => _hours;
            set
            {
                _hours = value;
                OnPropertyChanged(nameof(Hours));
            }
        }

        private bool _visibl = false;
        public bool Visibl
        {
            get => _visibl;
            set
            {
                _visibl = value;
                OnPropertyChanged(nameof(Visibl));
            }
        }

        private bool _visibl1 = false;
        public bool Visibl1
        {
            get => _visibl1;
            set
            {
                _visibl1 = value;
                OnPropertyChanged(nameof(Visibl1));
            }
        }

        private string _but = "Найти варианты";
        public string But
        {
            get => _but;
            set
            {
                _but = value;
                OnPropertyChanged(nameof(But));
            }
        }

        [RelayCommand]
        public void BookingCreate()
        {
            
        }
    }
}
