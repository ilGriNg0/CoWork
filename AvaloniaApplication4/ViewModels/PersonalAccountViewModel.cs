using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.IO;
using static AvaloniaApplication4.ViewModels.BusinessAccountViewModel;
using Tmds.DBus.Protocol;
using System.Runtime.CompilerServices;

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

        //[ObservableProperty]
        //private static bool _pressed;

        private static bool _pressed;
        public static bool Pressed
        {
            get
            {
                if (_pressed == null)
                {
                    _pressed = false;
                }
                return _pressed;
            }
            set => _pressed = value;
        }

        //public ObservableCollection<IdCompany> idCompanies { get; set; } = new ObservableCollection<IdCompany>();

        public string cs = User.Connect;
    
        public PersonalAccountViewModel()
        {
            //GetPhoto();
            GetInfo();
            //ReadBdCompany();
            //ReadPhotoBd();
            GetBookings();
           
            AddInfo();
            AddBook(BookingsLast);
            AddBook(Bookings);

        }
        //private static PersonalAccountViewModel? _instance;
        //public static PersonalAccountViewModel Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            _instance = new PersonalAccountViewModel();
        //        }
        //        return _instance;
        //    }
        //}
        //public void GetPhoto()
        //{
        //    try
        //    {

        //        var con = new NpgsqlConnection(cs);
        //        con.Open();

        //        var sql = $"SELECT img FROM main_users WHERE id = '{User.Id}';";

        //        var cmd = new NpgsqlCommand(sql, con);
        //        NpgsqlDataReader rdr = cmd.ExecuteReader();
        //        while(rdr.Read())
        //        {
        //            PhotoPath = new Bitmap(rdr.GetString(0));
        //        }

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

        public void GetInfo()
        {

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
                PhotoPath = new Bitmap(rdr.GetString(7));
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
        public Dictionary<(int, string), int> BookingValuePairs = new Dictionary<(int, string), int>();
        public Dictionary<(int, int), string> PhotoIDPathPairs = new();
        private readonly string _book1 = "bookingsLast";
        private readonly string _book2 = "bookings";
        public void GetBookings()
        {
            var con = new NpgsqlConnection(cs);
            con.Open();
           
            var sql = $"SELECT * FROM main_bookings WHERE id_user_id = '{User.Id}' ORDER BY date, time_start;";
            var cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            var bookings = new List<Booking>();
            var bookingsLast = new List<Booking>();

            while (rdr.Read())
            {
                if (rdr.GetDateTime(7) < DateTimeOffset.Now.Date || (rdr.GetTimeSpan(6) < DateTime.Now.TimeOfDay && rdr.GetDateTime(7) == DateTime.Now.Date))
                {
                    bookingsLast.Insert(0, new Booking(rdr.GetDateTime(7), rdr.GetTimeSpan(5), rdr.GetTimeSpan(6), rdr.GetInt32(1), rdr.GetInt64(3), rdr.GetInt32(9), rdr.GetInt32(0)));
                    BookingValuePairs.Add((rdr.GetInt32(0), _book1), rdr.GetInt32(1));
                    Visibl2 = false;
                }
                else
                {
                    bookings.Add(new Booking(rdr.GetDateTime(7), rdr.GetTimeSpan(5), rdr.GetTimeSpan(6), rdr.GetInt32(1), rdr.GetInt64(3), rdr.GetInt32(9), rdr.GetInt32(0)));
                    BookingValuePairs.Add((rdr.GetInt32(0), _book2), rdr.GetInt32(1));
                    Visibl1 = false;
                }
            }

            Bookings = new ObservableCollection<Booking>(bookings);
            BookingsLast = new ObservableCollection<Booking>(bookingsLast);
            //AddBook(Bookings);

            rdr.Close();
            con.Close();
        }
        public void AddBook(ObservableCollection<Booking> booking)
        {
            foreach (var item in PhotoIDPathPairs)
            {
                foreach (var book in booking)
                {
                    if (item.Key.Item2 == book.id_coworking)
                    {
                        book.B_maps.Add(new Bitmap(item.Value));
                    }
                }
            }

        }

        public async void ReadPhotoBd()
        {
            await using (var connect = new NpgsqlConnection(cs))
            {
                connect.Open();
                await using (var command = new NpgsqlCommand("SELECT * FROM main_images", connect))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int id_coworking = reader.GetInt32(1);
                        string str = reader.GetString(2);
                        PhotoIDPathPairs.Add((id, id_coworking), str);
                    }
                    reader.Close();
                }
            }
        }
        public void AddInfo()
        {

            //var card1ViewModel = new CardViewModel();
            var instancecard = (CardViewModel)Activator.CreateInstance(typeof(CardViewModel), true);
            foreach (var item in instancecard.Card_Collection)
            {
                foreach (var item2 in BookingValuePairs)
                {
                    if (item.Id == item2.Value)
                    {
                        if (item2.Key.Item2 == _book1)
                        {

                            foreach (var item3 in BookingsLast.Where(p => p.id_coworking == item2.Value))
                            {
                                item3.Name_Cowork = item?.Name_cowork;
                                foreach (var itm in item.Photos)
                                {
                                    item3.B_maps.Add(itm);
                                }

                            }
                           
                        }
                        else
                        {
                            foreach (var item3 in Bookings.Where(p => p.id_coworking == item2.Value))
                            {
                                item3.Name_Cowork = item?.Name_cowork;
                                foreach (var itm in  item.Photos)
                                {
                                    item3.B_maps.Add(itm);
                                }
                            }
                        }

                    }
                }
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
                sql = $"UPDATE main_users SET img = '{filepath}' WHERE id = '{User.Id}';";

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
    public partial class Booking : ObservableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int Id { get; set; }
        public string Name_Cowork { get; set; }
        [ObservableProperty]
        private Bitmap _path_cowork;
        public string? Date { get; set; }
        public string? Time { get; set; }
        private int _rating;
        public int Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged();
                }
            }
        }
        [ObservableProperty]
        private ObservableCollection<Bitmap> _b_maps = new();
        public int id_coworking { get; set; }

        public Booking() { }
        public Booking(string name)
        {
            Name_Cowork = name;
        }
        public Booking(DateTime date, TimeSpan time_start, TimeSpan time_end, int id_c, long price, int rating, int id)
        {
            id_coworking = id_c;
            Date = date.ToString("dd.MM.yyyy");
            Time = $"{time_start.ToString(@"hh\:mm")}-{time_end.ToString(@"hh\:mm")}" + $"   {price} ₽";
            Rating = rating;
            Id = id;
        }

       
    }
}
