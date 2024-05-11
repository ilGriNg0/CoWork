using Avalonia.Controls;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Npgsql;
using System;
using System.Diagnostics.Tracing;

namespace AvaloniaApplication4.ViewModels
{
    public partial class PersonalAccountViewModel : ViewModelBase
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
            var cs = "Host=localhost;Port=5432;Database=coworking;Username=postgres;Password=NoSmoking";
            var sql = $"SELECT * FROM main_users WHERE id = '{User.Id}';";
            var con = new NpgsqlConnection(cs);
            con.Open();
            var cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Email = rdr.GetString(1);
                Password = rdr.GetString(2);
                First = rdr.GetString(3);
                Last = rdr.GetString(4);
                Phone = rdr.GetString(5).Split("+7")[1].Replace("-","");
                Date = rdr.GetDateTime(6).ToString("dd/MM/yyyy");
            }
        }
    }
}
