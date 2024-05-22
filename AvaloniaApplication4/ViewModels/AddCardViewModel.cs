using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
namespace AvaloniaApplication4.ViewModels
{
    public partial class AddCardViewModel : ViewModelBase
    {
        [ObservableProperty]
        private JsonClass _json = new();

        [ObservableProperty]
        private string _errorMessage;
        [ObservableProperty]
        private Bitmap _photo_Coworking;
        public ObservableCollection<JsonClass> jsonClasses { get; set; } = new ObservableCollection<JsonClass>();
        public ICommand SaveCommand => new RelayCommand(add_collect);

        public ICommand AddPhotoCommand => new RelayCommand(add_photo);
        public AddCardViewModel() { }
        ConnectingBD connect = new();
        public void add_collect()
        {
          
            CardViewModel card = new();
            int counter = card.Card_Collection.Count + 1;
            var item = new JsonClass { Id = counter, Info_cowork = Json.Info_cowork, Location_cowork = Json.Location_cowork, Name_cowork = Json.Name_cowork, Price_day_cowork = Json.Price_day_cowork, Price_meetingroom_cowork = Json.Price_meetingroom_cowork };
            if ( item.Info_cowork != null && item.Location_cowork != null && item.Name_cowork != null && item.Price_day_cowork != null && item.Price_meetingroom_cowork != null)
            {
                jsonClasses.Add(item);
                connect?.WriteBd(jsonClasses);
                card.Card_Collection.Add(jsonClasses);
              
            }
            else
            {
                ErrorMessage = "Заполните поля";
            }

        }
        private readonly Window _target = new();
        public static FilePickerFileType ImageAll { get; } = new("All Images")
        {
            Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.gif", "*.bmp", "*.webp" },
            AppleUniformTypeIdentifiers = new[] { "public.image" },
            MimeTypes = new[] { "image/*" }
        };
        public async void add_photo() 
        {
            CardViewModel cardView = new();
          
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {

                //You can add either custom or from the built-in file types. See "Defining custom file types" on how to create a custom one.
                FileTypeFilter = new[] { ImageAll, FilePickerFileTypes.TextPlain }
            });
            if (files != null && files.Count > 0)
            {
                var selectedFile = files[0];
                var filePath = selectedFile.Path.LocalPath;
   
                connect.WriteBusinessBd(filePath, cardView.Card_Collection.Count + 1);
                Photo_Coworking = new Bitmap(filePath);
                Json.Path_photo = Photo_Coworking;
               
            }
               
        }

       
    }
}
