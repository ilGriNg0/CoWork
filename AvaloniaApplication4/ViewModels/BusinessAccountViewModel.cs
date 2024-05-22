using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Input;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

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

        [ObservableProperty]
        public string _company;
        [ObservableProperty]
        public string _email;
        [ObservableProperty]
        public string _phone;
        [ObservableProperty]
        public string _password;

        public ICommand NavigateToAddCommand => new RelayCommand<string>(NavigateToAdd);

        
        public void NavigateToAdd(string? pageViewModel)
        {
    
            var mainwindow = MainWindowViewModel.Instance;
            mainwindow.Navigate(pageViewModel);

        }
        public ObservableCollection<string> Coworkings { get; set; }
        public ICommand ButtonCommand { get; }

        public BusinessAccountViewModel()
        {
            GetPhoto();
            GetInfo();
            GetBookings();
            ButtonCommand = new Relay1Command(ButtonClick);
        }

        private bool isregphoto = false;
        public void GetPhoto()
        {
            try
            {
                var cs = User.Connect;
                var con = new NpgsqlConnection(cs);
                con.Open();

                var sql = $"SELECT file FROM main_images WHERE id_coworking = '{User.Id}';";

                var cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    PhotoPath = new Bitmap(rdr.GetString(0));
                    isregphoto = true;
                    return;
                }
                PhotoPath = new Bitmap("Assets\\nophotop1.png");
            }
            catch (Exception){}
            
        }

        private ObservableCollection<Booking> _bookings;
        private List<ObservableCollection<Booking>> _books = new List<ObservableCollection<Booking>>();
        private ObservableCollection<Booking> Bookings
        {
            get => _bookings;
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
            }
        }

        private void ButtonClick(object parameter)
        {
            if (parameter is string type)
            {
                Bookings = _books[Coworkings.IndexOf(type)];
            }
        }

        public void GetBookings()
        {
            var cs = User.Connect;
            var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = $"SELECT * FROM main_bookings WHERE id_coworking IN (SELECT id FROM main_coworkingspaces WHERE id_company = '{User.Id}') ORDER BY id_coworking, date_start;";
            
            var cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            var onelist = new List<Booking>();
            var cowors = new List<string>();
            while (rdr.Read())
            {
                if (!cowors.Contains(rdr.GetInt64(1).ToString()))
                {
                    if (onelist.Count > 0)
                    {
                        _books.Add(new ObservableCollection<Booking>(onelist));
                        onelist = new List<Booking>();
                    }
                    cowors.Add(rdr.GetInt64(1).ToString());
                }
                onelist.Add(new Booking(rdr.GetInt64(0), rdr.GetInt64(1), rdr.GetInt64(2), rdr.GetInt64(3), rdr.GetString(4), rdr.GetDateTime(5), rdr.GetDateTime(6)));

            }
            _books.Add(new ObservableCollection<Booking>(onelist));
            Bookings = _books[0];
            Coworkings = new ObservableCollection<string>(cowors);
            rdr.Close();
            con.Close();
        }

        public void GetInfo()
        {
            var cs = User.Connect;
            var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = $"SELECT * FROM main_businesses WHERE id = '{User.Id}';";
            var cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Company = rdr.GetString(1);
                User.Email = Email = rdr.GetString(2);
                User.Password = Password = rdr.GetString(3);
                User.Phone = Phone = rdr.GetString(4).Split("+7")[1].Replace("-", "");
            }
            rdr.Close();
            con.Close();
        }

        private class Booking
        {
            public long Id { get; set; }
            public long Id_coworking { get; set; }
            public long Id_user { get; set; }
            public long Price { get; set; }
            public string Type { get; set; }
            public DateTime Date_start { get; set; }
            public DateTime Date_end { get; set; }

            public Booking(long id, long id_coworking, long id_user, long price, string type, DateTime date_start, DateTime date_end)
            {
                Id = id;
                Id_coworking = id_coworking;
                Id_user = id_user;
                Price = price;
                Type = type;
                Date_start = date_start;
                Date_end = date_end;
            }
        }

        public ICommand PhotoClickedCommand => new RelayCommand(add_photo);

        private readonly Window _target = new();
        public static FilePickerFileType ImageAll { get; } = new("All Images")
        {
            Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.gif", "*.bmp", "*.webp" },
            AppleUniformTypeIdentifiers = new[] { "public.image" },
            MimeTypes = new[] { "image/*" }
        };
        public async void add_photo()
        {

            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                //You can add either custom or from the built-in file types. See "Defining custom file types" on how to create a custom one.
                FileTypeFilter = new[] { ImageAll, FilePickerFileTypes.TextPlain }
            });
            if (files != null && files.Count > 0)
            {
                // Получаем путь к первому выбранному файлу
                var selectedFile = files[0];
                var filePath = selectedFile.Path.LocalPath;

                var cs = User.Connect;
                var con = new NpgsqlConnection(cs);
                string sql;

                con.Open();
                if (isregphoto)
                    sql = $"UPDATE main_images SET file = '{filePath}' WHERE id_coworking = '{User.Id}';";
                else 
                    sql = $"INSERT INTO main_images(id_coworking, file) VALUES('{User.Id}', '{filePath}');";

                var cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                PhotoPath = new Bitmap(filePath);
            }
        }

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
    }
}
