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
namespace AvaloniaApplication4.ViewModels
{
    public partial class AddCardViewModel : ViewModelBase
    {
        [ObservableProperty]
        private JsonClass _json = new();

        [ObservableProperty]
        private string _errorMessage;

        public ObservableCollection<JsonClass> jsonClasses { get; set; } = new ObservableCollection<JsonClass>();
        public ICommand SaveCommand => new RelayCommand(add_collect);

        public ICommand AddPhotoCommand => new RelayCommand(add_photo);
        public AddCardViewModel() { }
        public void add_collect()
        {
            ConnectingBD connect = new();
            var item = new JsonClass { Id = Json.Id, Info_cowork = Json.Info_cowork, Location_metro_cowork = Json.Location_metro_cowork, Name_cowork = Json.Name_cowork, Price_day_cowork = Json.Price_day_cowork, Price_meetingroom_cowork = Json.Price_meetingroom_cowork };
            if (item.Info_cowork != null && item.Location_metro_cowork != null && item.Name_cowork != null && item.Price_day_cowork != null && item.Price_meetingroom_cowork != null)
            {
                jsonClasses.Add(item);
                //connect?.WriteBd(jsonClasses);
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

            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {

                //You can add either custom or from the built-in file types. See "Defining custom file types" on how to create a custom one.
                FileTypeFilter = new[] { ImageAll, FilePickerFileTypes.TextPlain }
            });
            var path_photo = new JsonClass {Path_photo = Json.Path_photo };
            jsonClasses.Add(path_photo);
        }

       
    }
}
