using Avalonia.Controls;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Kernel;
using Npgsql;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
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
        [ObservableProperty]
        private ObservableCollection<JsonClass> _borderCompany1 = new();

        [ObservableProperty]
        private List<JsonClass> _borderCompany2 = new();

        [ObservableProperty]
        private List<JsonClass> _borderCompany3 = new();
        public ObservableCollection<IdCompany> idCompanies { get; set; } = new ObservableCollection<IdCompany>();
        public PersonalAccountViewModel()
        {
            GetInfo();
            ReadBdCompany();
            AddInfo();
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

        public string? Name { get; set; }
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
                    bookingsLast.Insert(0, new Booking(rdr.GetDateTime(5), rdr.GetDateTime(6), Name));
                    Visibl2 = false;
                }
                else
                {
                    bookings.Add(new Booking(rdr.GetDateTime(5), rdr.GetDateTime(6), Name));
                    Visibl1 = false;
                }
            }
        
            Bookings = new ObservableCollection<Booking>(bookings);
            BookingsLast = new ObservableCollection<Booking>(bookingsLast);
            rdr.Close();
            con.Close();
        }
        public async Task ReadBdCompany()
        {

            //List<object> types = [];
            string connect_host = User.Connect;
            int id = default;
            await using (var connect = new NpgsqlConnection(connect_host))
            {
                connect.Open();
                await using (var command = new NpgsqlCommand("SELECT * FROM main_bookings", connect))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt32(1);
                        var item = new IdCompany { Id_Company = id };
                        idCompanies.Add(item);

                    }
                    reader.Close();
                }
            }
        }
        public void AddInfo()
        {
            CardViewModel card1ViewModel = new CardViewModel();
            foreach (var item in idCompanies)
            {
                switch (item.Id_Company)
                {
                    case 1:
                        foreach (var item2 in card1ViewModel.PeopleCollection)

                           if(item2.Id == item.Id_Company)
                            {
                               Booking booking = new Booking();
                                Name = item2.Name_cowork;
                            }
                            break;
                    case 2:
                        foreach (var item2 in card1ViewModel.PeopleCollection)

                            if (item2.Id == item.Id_Company)
                            {
                                Booking booking = new Booking();
                                Name = item2.Name_cowork;
                            }
                        break;
                    case 3:
                        foreach (var item2 in card1ViewModel.PeopleCollection)

                            if (item2.Id == item.Id_Company)
                            {
                                Booking booking = new Booking();
                                Name = item2.Name_cowork;
                            }
                        break;
                    case 4:
                        foreach (var item2 in card1ViewModel.PeopleCollection)

                            if (item2.Id == item.Id_Company)
                            {
                                Booking booking = new Booking();
                                Name = item2.Name_cowork;
                            }
                        break;  
                    default:
                        break;
                }
            }
        }

        private class Booking
        {
            public string? NameCowork { get; set; }  
            public string? Path {  get; set; }
            public string? Date { get; set; }
            public string? Time { get; set; }

            public Booking() { }
            public Booking(DateTime date_start, DateTime date_end, string Coworkname)
            {
                NameCowork = Coworkname;
                Date = date_start.ToString("dd.MM.yyyy");
                Time = $"{date_start.ToString("HH:mm")}-{date_end.ToString("HH:mm")}";
            }
        }
    }
}
