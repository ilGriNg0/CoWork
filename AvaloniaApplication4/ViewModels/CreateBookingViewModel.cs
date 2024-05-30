using Avalonia.Controls;
using Avalonia.Media.Imaging;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HarfBuzzSharp;
using MsBox.Avalonia.Base;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvaloniaApplication4.ViewModels.PersonalAccountViewModel;

namespace AvaloniaApplication4.ViewModels
{
    public partial class CreateBookingViewModel : ViewModelBase
    {
        private Bitmap _photoPath;
        public Bitmap PhotoPath
        {
            get => _photoPath;
            set
            {
                if (_photoPath != value)
                {
                    _photoPath = value;
                    OnPropertyChanged();
                }
            }
        }
        private int choise;
        private int Id_c = 1;
        private int Open = 9;
        private int Closed = 23;

        private List<int> Count = new List<int>();
        private List<long> Price = new List<long>();
        private List<Bitmap> Img = new List<Bitmap>();
        public CreateBookingViewModel() {}
        public CreateBookingViewModel(int id_с, int open, int closed) 
        {
            //User.Id = 1;
            Id_c = id_с;
            Open = open;
            Closed = closed;

            GetServices();

            List<string> hrs = new List<string>();
            for (int i = 1; i <= Closed - Open; i++)
            {
                hrs.Add(i > 4 ? $"{i} часов" : i < 5 ? $"{i} часа" : $"{i} час");
            }
            Hours = new ObservableCollection<string>(hrs);
            SelectedHour = Hours[0];
        }

        private void GetServices()
        {
            List<string> tps = new List<string>();

            var cs = User.Connect;
            var con = new NpgsqlConnection(cs);
            con.Open();
            var sql = $"SELECT * FROM main_services WHERE id_coworking_id = '{Id_c}';";
            var cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            int i = 0;
            while (rdr.Read())
            {
                Price.Add(rdr.GetInt64(2));
                tps.Add($"{rdr.GetString(3)}  {Price[i]} ₽/час");
                Count.Add(rdr.GetInt32(4));
                Img.Add(new Bitmap(rdr.GetString(5)));
                i++;
            }
            Types = new ObservableCollection<string>(tps);
            SelectedType = Types[0];
            PhotoPath = Img[0];
        }

        private ObservableCollection<string> _types;
        public ObservableCollection<string> Types
        {
            get => _types;
            set
            {
                _types = value;
                OnPropertyChanged(nameof(Types));
            }
        }

        private string _selectedType = "";
        public string SelectedType
        {
            get => _selectedType;
            set
            {
                if (_selectedType != value)
                {
                    choise = Types.IndexOf(value);
                    _selectedType = value;
                    PhotoPath = Img[choise];
                    OnPropertyChanged(nameof(SelectedType));
                    if (SelectedHour != "")
                        GetFreeTimes();
                }
            }
        }

        private DateTimeOffset _selectedDate = new DateTimeOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
        public DateTimeOffset SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (value != _selectedDate)
                {
                    if (value < DateTime.Now.Date)
                        _selectedDate = new DateTimeOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                    else
                        _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    if (SelectedHour != "" && SelectedType != "")
                        GetFreeTimes();
                }
            }
        }

        private ObservableCollection<string> _hours;
        public ObservableCollection<string> Hours
        {
            get => _hours;
            set
            {
                _hours = value;
                OnPropertyChanged(nameof(Hours));
            }
        }

        private string _selectedHour = "";
        public string SelectedHour
        {
            get => _selectedHour;
            set
            {
                if (_selectedHour != value)
                {
                    _selectedHour = value;
                    OnPropertyChanged(nameof(SelectedHour));
                    if (SelectedType != "")
                        GetFreeTimes();
                }
            }
        }

        private ObservableCollection<string> _times;
        public ObservableCollection<string> Times
        {
            get => _times;
            set
            {
                _times = value;
                OnPropertyChanged(nameof(Times));
            }
        }

        private string _selectedTime = "";
        public string SelectedTime
        {
            get => _selectedTime;
            set
            {
                _selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
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

        private Dictionary<string, int> ft;

        private void GetFreeTimes()
        {
            Dictionary<int, List<int>> nft = new Dictionary<int, List<int>>();

            ft = new Dictionary<string, int>();

            List<Booking> bookings = new List<Booking>();

            var cs = User.Connect;

            var con = new NpgsqlConnection(cs);
            con.Open();

            int hours = Int32.Parse(SelectedHour.Split(" ")[0]);
            var sql = $"SELECT time_start, time_end, number FROM main_bookings WHERE id_coworking_id = '{Id_c}' AND type = '{SelectedType.Split("  ")[0]}' AND date = '{SelectedDate}' ORDER BY time_start;";
            var cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                bookings.Add(new Booking(rdr.GetTimeSpan(0).Hours, rdr.GetTimeSpan(1).Hours, rdr.GetInt32(2)));
            }

            if (bookings.Count == 0)
            {
                for (int i = SelectedDate == DateTimeOffset.Now.Date && DateTimeOffset.Now.Hour >= Open ? DateTime.Now.Hour + 1 : Open; i <= Closed - hours; i++)
                {
                    ft.Add($"{i} - {i + hours}", 1);
                }
            }
            else
            {
                foreach (var book in bookings)
                {
                    if (!nft.ContainsKey(book.number))
                    {
                        nft.Add(book.number, new List<int>());
                    }
                    for (int i = book.end - 1; i >= book.start; i--)
                    {
                        nft[book.number].Add(i);
                    }
                }

                foreach (var nf in nft)
                {
                    if (!((SelectedDate == DateTimeOffset.Now.Date && DateTimeOffset.Now.Hour >= Open ? DateTime.Now.Hour + 1 - Open : Closed - Open) - nf.Value.Count < Int32.Parse(SelectedHour.Split(" ")[0])))
                    {
                        for (int i = SelectedDate == DateTimeOffset.Now.Date && DateTimeOffset.Now.Hour >= Open ? DateTime.Now.Hour + 1 : Open; i <= Closed - hours; i++)
                        {
                            bool free = true;
                            for (int j = i; j < i + hours; j++)
                            {
                                if (nf.Value.Contains(j))
                                {
                                    free = false;
                                    break;
                                }
                            }
                            if (free && !ft.ContainsKey($"{i} - {i + hours}")) ft.Add($"{i} - {i + hours}", nf.Key);
                        }
                    }
                }

                if (nft.Count < Count[choise])
                {
                    int choises = 0;
                    for (int i = 1; i <= Count[choises]; i++)
                    {
                        if (!nft.ContainsKey(i))
                        {
                            choises = i;
                            break;
                        }
                    }
                    for (int i = SelectedDate == DateTimeOffset.Now.Date && DateTimeOffset.Now.Hour >= Open ? DateTime.Now.Hour + 1 : Open; i <= Closed - hours; i++)
                    {
                        if (!ft.ContainsKey($"{i} - {i + hours}"))
                            ft.Add($"{i} - {i + hours}", choises);
                    }
                }
            }
            var keys = new List<string>(ft.Keys);
            for(int j = 0; j < ft.Count; j++)
                for (int i = 0; i < ft.Count - 1; i++)
                {
                    if (Int32.Parse(keys[i].Split("-")[0]) > Int32.Parse(keys[i+1].Split("-")[0]))
                    {
                        var buuf = keys[i];
                        keys[i] = keys[i+1];
                        keys[i+1] = buuf;
                    }
                }

            Times = new ObservableCollection<string>(keys);
            if (Times.Count == 0) 
            {
                SelectedTime = "";
                Visibl1 = true;
                Visibl = false;
            }
            else
            {
                Visibl = true;
                Visibl1 = false;
                SelectedTime = Times[0];
            }
        }

        [RelayCommand]
        public void BookingCreate()
        {
            if (SelectedTime != "" && Visibl) 
            {
                var cs = User.Connect;

                var con = new NpgsqlConnection(cs);
                con.Open();
                var sql = $"INSERT INTO main_bookings (id_coworking_id, id_user_id, price, type, time_start, time_end, date, number, rating)" +
                    $"VALUES ('{Id_c}', '{User.Id}', '{Price[choise] * Int32.Parse(SelectedHour.Split(" ")[0])}', '{SelectedType.Split("  ")[0].Trim()}', '{TimeSpan.FromHours(Int32.Parse(SelectedTime.Split("-")[0]))}', " +
                    $"'{TimeSpan.FromHours(Int32.Parse(SelectedTime.Split("-")[1]))}', '{SelectedDate}', '{ft[SelectedTime]}', 0);";
                var cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
            }
        }

        public class Booking
        {
            public int start;
            public int end;
            public int number;

            public Booking(int s, int e, int n)
            {
                start = s;
                end = e;
                number = n;
            }
        }
    }
}
