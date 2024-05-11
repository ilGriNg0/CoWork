using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace AvaloniaApplication4.ViewModels
{
    public partial class BusinessAccountViewModel : ViewModelBase
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

        public ObservableCollection<Booking> Bookings { get; }

        [ObservableProperty]
        public string _company;
        [ObservableProperty]
        public string _email;
        [ObservableProperty]
        public string _phone;
        [ObservableProperty]
        public string _password;
        

        public BusinessAccountViewModel()
        {
            var cs = "Host=localhost;Port=5432;Database=coworking;Username=postgres;Password=NoSmoking";
            var sql = $"SELECT * FROM main_bookings;";
            var con = new NpgsqlConnection(cs);
            con.Open();
            var cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            var bookings = new List<Booking>();
            while (rdr.Read())
            {
                bookings.Add(new Booking(rdr.GetInt64(0), rdr.GetInt64(1), rdr.GetInt64(2), rdr.GetInt64(3), rdr.GetString(4), rdr.GetDateTime(5), rdr.GetDateTime(6)));
            }
            Bookings = new ObservableCollection<Booking>(bookings);
            rdr.Close();
            sql = $"SELECT * FROM main_businesses WHERE id = '{User.Id}';";
            cmd = new NpgsqlCommand(sql, con);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Company = rdr.GetString(1);
                User.Email = Email = rdr.GetString(2);
                User.Password = Password = rdr.GetString(3);
                User.Phone = Phone = rdr.GetString(4).Split("+7")[1].Replace("-", "");
            }
            con.Close();
        }
        public class Booking
        {
            public long Id { get; set; }
            public long Id_coworking { get; set; }
            public long Id_user { get; set; }
            public long Price { get; set; }
            public string Type { get; set; }
            public DateTime Date_start { get; set; }
            public DateTime Date_end { get; set; }

            public Booking(long id, long id_booking, long id_user, long price, string type, DateTime date_start, DateTime date_end)
            {
                Id = id;
                Id_coworking = id_booking;
                Id_user = id_user;
                Price = price;
                Type = type;
                Date_start = date_start;
                Date_end = date_end;
            }
        }
    }
}
