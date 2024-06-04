using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
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
        [ObservableProperty]
        private static bool _keyBusin;

        public ICommand NavigateToAddCommand => new RelayCommand<string>(NavigateToAdd);

        
        public void NavigateToAdd(string? pageViewModel)
        {
    
            var mainwindow = MainWindowViewModel.Instance;
            mainwindow.Navigate(pageViewModel);

        }
        private static bool _key_boookingPressed;

        public static bool Key_boookingPressed
        {
            get
            {
                if (_key_boookingPressed == null)
                {
                    _key_boookingPressed = false;
                }
                return _key_boookingPressed;
            }
            set => _key_boookingPressed = value;



        }
        private static BusinessAccountViewModel? _instance;

        public static BusinessAccountViewModel Instance
        {
         get
            {
             if(_instance == null)
                {
                    _instance = new BusinessAccountViewModel();
                }
                return _instance;
            }
            
           
            
        }
        [ObservableProperty]
        private ObservableCollection<JsonClass> _bookingsBusines = new();
        public ObservableCollection<string> Coworkings { get; set; }
        public ICommand ButtonCommand { get; }
        ConnectingBD connect = new();
        public BusinessAccountViewModel()
        {
            //GetPhoto();
            GetInfo();
            GetBookings();
            connect.ReadPhotoBusinessBd();
            InsertBookings();
            ButtonCommand = new Relay1Command(ButtonClick);
        }

        //private bool isregphoto = false;
        //public void GetPhoto()
        //{
        //    try
        //    {
        //        var cs = User.Connect;
        //        var con = new NpgsqlConnection(cs);
        //        con.Open();

        //        var sql = $"SELECT img FROM main_images WHERE id_coworking = '{User.Id}';";

        //        var cmd = new NpgsqlCommand(sql, con);
        //        NpgsqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            isregphoto = true;
        //            if (File.Exists(rdr.GetString(0)))
        //                PhotoPath = new Bitmap(rdr.GetString(0));
        //            else goto _L1;
        //            return;
        //        }
        //    _L1: string filepath = AppContext.BaseDirectory + "Assets\\nophotop1.png";
        //        if (!File.Exists(filepath))
        //        {
        //            Directory.CreateDirectory(filepath.Replace("\\nophotop1.png", ""));
        //            File.Copy(filepath.Replace("\\bin\\Debug\\net8.0", ""), filepath);
        //        }
        //        PhotoPath = new Bitmap(filepath);
        //    }
        //    catch (Exception) { }
        //}

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

            //var sql = $"SELECT * FROM main_bookings WHERE id_coworking_id IN (SELECT id FROM main_coworkingspaces WHERE id_company_id = '{User.Id}') ORDER BY id_coworking_id, date;";
            //var sql = $"SELECT o.id, o.price, o.type, o.time_start, o.time_end, o.date, o.number, o.rating, c.first_name, c.last_name, c.phone_number, p.coworking_name " +
            //    $"FROM main_bookings o " +
            //    $"JOIN main_users c ON o.id_user_id = c.id " +
            //    $"JOIN main_coworkingspaces p ON p.id_company_id = '{User.Id}' " +
            //    $"WHERE o.id_coworking_id = '{User.Id}' ORDER BY p.coworking_name, o.date;";

            //var sql = $"SELECT o.id, o.price, o.type, o.time_start, o.time_end, o.date, o.number, o.rating, c.first_name, c.last_name, c.phone_number, p.coworking_name\r\nFROM (\r\n    SELECT DISTINCT o.id, o.price, o.type, o.time_start, o.time_end, o.date, o.number, o.rating, o.id_user_id, o.id_coworking_id\r\n    FROM main_bookings o\r\n    WHERE o.id_coworking_id = '{User.Id}'\r\n) o\r\nJOIN main_users c ON o.id_user_id = c.id\r\nJOIN main_coworkingspaces p ON o.id_coworking_id = p.id_company_id\r\nORDER BY o.id_coworking_id, o.date;";
            var sql = $"SELECT o.id, o.price, o.type, o.time_start, o.time_end, o.date, o.number, o.rating, c.first_name, c.last_name, c.phone_number, p.coworking_name\r\nFROM main_coworkingspaces p\r\nJOIN main_bookings o ON o.id_coworking_id = p.id\r\nJOIN main_users c ON o.id_user_id = c.id\r\nWHERE p.id_company_id = '{User.Id}' ORDER BY p.coworking_name, o.date;";
            var cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            var onelist = new List<Booking>();
            var cowors = new List<string>();
            while (rdr.Read())
            {
                if (!cowors.Contains(rdr.GetString(11)))
                {
                    if (onelist.Count > 0)
                    {
                        _books.Add(new ObservableCollection<Booking>(onelist));
                        onelist = new List<Booking>();
                    }
                    cowors.Add(rdr.GetString(11));
                }
                onelist.Add(new Booking(rdr.GetInt32(0), rdr.GetInt64(1), rdr.GetString(2), $"{rdr.GetTimeSpan(3).ToString(@"hh\:mm")}-{rdr.GetTimeSpan(4).ToString(@"hh\:mm")}",
                    rdr.GetDateTime(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetString(8), rdr.GetString(9), rdr.GetString(10)));

            }
            _books.Add(new ObservableCollection<Booking>(onelist));
            Bookings = _books[0];
            Coworkings = new ObservableCollection<string>(cowors);
            rdr.Close();
            con.Close();
        }
        public void InsertBookings()
        {
           //var cardViewModel = new CardViewModel();
            var instanceBs = (CardViewModel)Activator.CreateInstance(typeof(CardViewModel), true);
            //var method = typeof(AddCardViewModel).GetMethod("OnDataLoad");
            //method.Invoke(instanceBs, parameters: null);
            foreach (var item in instanceBs.Card_Collection)
            {
              BookingsBusines.Add(item);
              insertPhotoBookings();
                    
            }
        }
        public void insertPhotoBookings()
        {
            ConnectingBD connectingdb = new ConnectingBD();
            connectingdb.ReadPhotoBusinessBd();
            foreach (var item in connectingdb.PhotoIDPathBusinessPairs)
            {
                    foreach (var item2 in BookingsBusines)
                    {
                      if(item.Key.Item2 == item2.Id)
                        {
                            item2.Photos.Add(new Bitmap(item.Value));
                        }  
                    }    
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
                PhotoPath = new Bitmap(rdr.GetString(5));
            }
            rdr.Close();
            con.Close();
        }

        private partial class Booking : ObservableObject
        {
            public int Id { get; set; }
            public string Type {  get; set; }
            public string Date {  get; set; }
            public string Time { get; set; }
            public int Number { get; set; }
            public string Price {  get; set; }
            public string First {  get; set; }
            public string Last { get; set; }
            public string Phone { get; set; }
            public int Rating { get; set; }


            //public int Id_coworking { get; set; }
            //public int Id_user { get; set; }
            //public string Price { get; set; }
            //public string Type { get; set; }
            //public string Time { get; set; }
            //public DateTime Date { get; set; }
            //public int Number {  get; set; }
       
             
 

            public Booking(int id, long price, string type, string time, DateTime date, int number, int rating, string first, string last, string phone)
            {
                Id = id;
                Rating = rating;
                Phone = phone;
                Number = number;
                First = first;
                Last = last;
                Price = $"{price} ₽";
                Type = type;
                Date = date.ToString("dd.MM.yyyy");
                Time = time;
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


                string filepath = "Assets\\" + Path.GetFileName(filePath);
                if (!File.Exists(filepath))
                {
                    File.Copy(filePath, filepath);
                }
                PhotoPath = new Bitmap(filepath);

                var cs = User.Connect;
                var con = new NpgsqlConnection(cs);
                string sql;

                con.Open();
                sql = $"UPDATE main_businesses SET img = '{filepath}' WHERE id = '{User.Id}';";

                var cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
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
