using Avalonia.Controls;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Kernel;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using static AvaloniaApplication4.ViewModels.BusinessAccountViewModel;

namespace AvaloniaApplication4.ViewModels
{
    public partial class PersonalAccountViewModel : ViewModelBase, INotifyPropertyChanged
    {
        [ObservableProperty]
        public char _charac = '#';
        private bool _isChecked = true;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    if (Charac == '#') Charac = '\0';
                    else Charac = '#';
                }
            }
        }

        [ObservableProperty]
        public string _first;
        [ObservableProperty]
        public string _last;
        [ObservableProperty]
        public string _email;
        [ObservableProperty]
        public string _phone;
        [ObservableProperty]
        public string _password;
        [ObservableProperty]
        public string _date;

        public PersonalAccountViewModel()
        {
            GetInfo();
            GetBookings();
        }

        public void GetInfo()
        {
            var cs = User.Connect;
            var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = $"SELECT * FROM main_users WHERE id = '{User.Id}';";
            var cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                User.Email = Email = rdr.GetString(1);
                User.Password = Password = rdr.GetString(2);
                First = rdr.GetString(3);
                Last = rdr.GetString(4);
                User.Phone = Phone = rdr.GetString(5).Split("+7")[1].Replace("-", "");
                Date = rdr.GetDateTime(6).ToString("dd/MM/yyyy");
            }
            rdr.Close();
            con.Close();
        }

        private ObservableCollection<Booking> Bookings { get; set; }
        private ObservableCollection<Booking> BookingsLast { get; set; }

        [ObservableProperty]
        private bool _visibl1 = true;
        [ObservableProperty]
        private bool _visibl2 = true;
        public void GetBookings()
        {
            var cs = User.Connect;
            var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = $"SELECT * FROM main_bookings WHERE id_user = '{User.Id}' ORDER BY date_start;";
            var cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            var bookings = new List<Booking>();
            var bookingsLast = new List<Booking>();
            while (rdr.Read())
            {
                if (rdr.GetDateTime(5) < DateTime.Now)
                {
                    bookingsLast.Insert(0, new Booking(rdr.GetDateTime(5), rdr.GetDateTime(6)));
                    Visibl1 = false;
                }
                else
                {
                    bookings.Add(new Booking(rdr.GetDateTime(5), rdr.GetDateTime(6)));
                    Visibl2 = false;
                }
            }
            Bookings = new ObservableCollection<Booking>(bookings);
            BookingsLast = new ObservableCollection<Booking>(bookingsLast);
            rdr.Close();
            con.Close();
        }

        private class Booking
        {
            public string Date { get; set; }
            public string Time { get; set; }

            public Booking(DateTime date_start, DateTime date_end)
            {
                Date = date_start.ToString("dd.MM.yyyy");
                Time = $"{date_start.ToString("HH:mm")}-{date_end.ToString("HH:mm")}";
            }
        }
    }
}
