using Avalonia.Controls.Shapes;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using AvaloniaApplication4.ViewModels;
using DynamicData;
using Microsoft.VisualBasic;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication4.Models
{
    public partial class ConnectingBD
    {
        string connect_host = User.Connect;
        public Dictionary<int, List<JsonClass>> keyValuePairs { get; set; } = new Dictionary<int, List<JsonClass>>();
        public Dictionary<int, List<Benefits>> keyValueBenefitsPairs { get; set; } = new Dictionary<int, List<Benefits>>();
        public Dictionary<(int, int), string> PhotoIDPathPairs = new();
        public List<Dictionary<(int, int), string>> MainPhotoIDPathPairs = new();
        public Dictionary<(int, int), string> PhotoIDPathBusinessPairs = new();
        public Dictionary<int, (int, int, int, string, string)> ServicesPairs = new();
        public Dictionary<int, (int, int, string)> MainServicesPairs = new();
        public List<int> BusinnessIdUsers = new();
        public ObservableCollection<Bitmap> Images { get; } = new();
        //public ObservableCollection<IdCompany> idCompanies { get; set; } = new ObservableCollection<IdCompany>();

        public async Task WriteBd(ObservableCollection<JsonClass> collection)
        {
            var json_collect = JsonConvert.SerializeObject(collection);
            await using (var connect = new NpgsqlConnection(connect_host))
            {
                connect.Open();
                string command_add = "INSERT INTO json_cards_info (json_cards) VALUES (@json)";
                await using (var command = new NpgsqlCommand(command_add, connect))
                {
                    command.Parameters.Add(new NpgsqlParameter("@json", NpgsqlDbType.Json) { Value = json_collect });
                    command.ExecuteNonQuery();
                }
            }
        }
        public async Task WriteBenefitsBd(ObservableCollection<Benefits> collection)
        {
            var json_collect = JsonConvert.SerializeObject(collection);
            await using (var connect = new NpgsqlConnection(connect_host))
            {
                connect.Open();
                string command_add = "INSERT INTO benefits (bests) VALUES (@json)";
                await using (var command = new NpgsqlCommand(command_add, connect))
                {
                    command.Parameters.Add(new NpgsqlParameter("@json", NpgsqlDbType.Json) { Value = json_collect });
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task ReadBenefitsBd<T>(T items)
        {
            string str = string.Empty;
           
            await using (var connect = new NpgsqlConnection(connect_host))
            {
                connect.Open();
                await using (var command = new NpgsqlCommand("SELECT * FROM benefits", connect))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id_BD = reader.GetInt32(0);
                        str = reader.GetString(1);
                        List<Benefits>? js = JsonConvert.DeserializeObject<List<Benefits>>(str);
                        if (js != null)
                        {
                            foreach (var item in js)
                            {
                                if (((items is JsonClass json) && json.Id == item.Id_Coworking ) || ((items is Booking book) && book.id_coworking == item.Id_Coworking))
                                {
                                    keyValueBenefitsPairs.Add(id_BD, js);
                                }
                                
                            }
                        }
                       
                        //types.AddRange(js);
                    }

                    reader.Close();
                }
            }
        }
        public string cs = User.Connect;
        public async Task ReadMainPhotoBd()
        {
            await using (var connect = new NpgsqlConnection(cs))
            {
                await connect.OpenAsync();
                await using (var command = new NpgsqlCommand("SELECT * FROM main_images", connect))
                {
                    try
                    {
                        var reader = command.ExecuteReader();
                        while (await reader.ReadAsync())
                        {
                            int id = reader.GetInt32(0);
                            int id_coworking = reader.GetInt32(1);
                            string str = reader.GetString(2);
                            //byte[] buf = new byte[reader.FieldCount];

                            PhotoIDPathPairs.Add((id, id_coworking), str);
                        }
                        reader.Close();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                  
                  
                    //foreach (var item in PhotoIDPathPairs)
                    //{
                    //    // Отфильтровываем элементы, где Item2 не совпадает с текущим item.Key.Item2
                    //    var nonMatchingItems = PhotoIDPathPairs
                    //        .Where(p => p.Key.Item2 != item.Key.Item2)
                    //        .ToDictionary(p => p.Key, p => p.Value);

                    //    // Добавляем отфильтрованный словарь в целевой список, если он не пуст
                    //    if (nonMatchingItems.Count > 0)
                    //    {
                    //        MainPhotoIDPathPairs.Add(nonMatchingItems);
                    //    }
                    //}

                    
                }
            }
        }
        //public async Task LoadImagesAsync()
        //{
        //    await ReadMainPhotoBd();
        //    object  images = new object();
        //    // Обработка изображений в отдельном потоке
        //    await Task.Run(async () =>
        //    {
        //        var tasks = PhotoIDPathPairs.Select(async kvp =>
        //        {
        //            var imagePath = kvp.Value;

        //            // Загружаем изображение асинхронно и конвертируем в Bitmap
        //            var imageData = await LoadImageDataFromPathAsync(imagePath);
        //            if (imageData != null)
        //            {
        //                using (var ms = new MemoryStream(imageData))
        //                {
        //                    var bitmap = new Bitmap(ms);
        //                    await Dispatcher.UIThread.InvokeAsync(() => Images.Add(bitmap));
        //                }
        //            }
        //        });

        //        await Task.WhenAll(tasks);
        //    });
        //}
        //private async Task<byte[]> LoadImageDataFromPathAsync(string path)
        //{
        //    if (File.Exists(path))
        //    {
        //        return await File.ReadAllBytesAsync(path);
        //    }
        //    else
        //    {
        //        throw new FileNotFoundException("Файл не найден", path);
        //    }
        //}

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
        public async void ReadPhotoBusinessBd()
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
                        PhotoIDPathBusinessPairs.Add((id, id_coworking), str);
                    }
                    reader.Close();
                }
            }
        }

        public async void ReadBusinessId(int idbsn)
        {

            await using (var connect = new NpgsqlConnection(cs))
            {
                connect.Open();
                await using(var command = new NpgsqlCommand("SELECT * FROM main_businesses",connect))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        if (id == idbsn)
                        {
                            BusinnessIdUsers.Add(id);
                        } 
                        
                    }    
                }
            }
        }
        public async Task WriteBusinessBd(ObservableCollection<string> bitmaps_path, int id)
        {

            await using (var connect = new NpgsqlConnection(connect_host))
            {
                connect.Open();
                string command_add = "INSERT INTO main_images (id_coworking_id, img) VALUES (@intValue, @text)";
                await using (var command = new NpgsqlCommand(command_add, connect))
                {
                   
                    foreach (var item in bitmaps_path)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new NpgsqlParameter("@intValue", NpgsqlDbType.Integer) { Value = id });
                        command.Parameters.Add(new NpgsqlParameter("@text", NpgsqlDbType.Text) { Value = item });
                        command.ExecuteNonQuery();
                    }
                  
                }
            }
        }
        public async Task WriteServicesBd(int id, ObservableCollection<JsonClass> js)
        {
            await using (var conncet = new NpgsqlConnection(connect_host))
            {
                conncet.Open();
                string command_add_serv = "INSERT INTO main_services (id_coworking_id, price, type, num_of_seats, img) VALUES (@id, @price1, @info, @count1, @icon);";
                                         
                await using (var command = new NpgsqlCommand(command_add_serv, conncet))
                {
                    foreach (var item in js)
                    {
                        command.Parameters.Clear(); 
                        command.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Integer) { Value = id });
                        command.Parameters.Add(new NpgsqlParameter("@price1", NpgsqlDbType.Integer) { Value = item.Tariffs_price });
                        command.Parameters.Add(new NpgsqlParameter("@count1", NpgsqlDbType.Integer) { Value = item.Tariffs_count });
                        command.Parameters.Add(new NpgsqlParameter("@info", NpgsqlDbType.Varchar) { Value = item.Tariffs });
                        command.Parameters.Add(new NpgsqlParameter("@icon", NpgsqlDbType.Varchar) { Value = "sdfsdf" });
                        command.ExecuteNonQuery();

                    }
                   

                  
                  
                }
            }
        }
        public async Task WriteNormalBD(int id_company, string info, string date_start, string date_end, string name, string address, int rating_ctn, int rating_sum, int businid)
        {
           
            //var json_collect2 = JsonConvert.SerializeObject(js);
            await using (var conncet = new NpgsqlConnection(connect_host))
            {
                conncet.Open();
                string command_add_serv = "INSERT INTO main_coworkingspaces (id_company_id, description, date_start, date_end, coworking_name, address, rating_count, rating_sum, busin_id) VALUES (@id_company, @info, @date_start, @date_end, @name, @address, @rating_ctn, @rating_sum, @businid);";
                await using (var command = new NpgsqlCommand(command_add_serv, conncet))
                {
                    command.Parameters.Add(new NpgsqlParameter("@id_company", NpgsqlDbType.Integer) { Value = id_company });
                    command.Parameters.Add(new NpgsqlParameter("@info", NpgsqlDbType.Varchar) { Value = info });
                    command.Parameters.Add(new NpgsqlParameter("@date_start", NpgsqlDbType.Varchar) { Value = date_start });
                    command.Parameters.Add(new NpgsqlParameter("@date_end", NpgsqlDbType.Varchar) { Value = date_end });
                    command.Parameters.Add(new NpgsqlParameter("@name", NpgsqlDbType.Varchar) { Value = name });
                    command.Parameters.Add(new NpgsqlParameter("@address", NpgsqlDbType.Varchar) { Value = address });
                    command.Parameters.Add(new NpgsqlParameter("@rating_ctn", NpgsqlDbType.Integer) { Value = rating_ctn });
                    command.Parameters.Add(new NpgsqlParameter("@rating_sum", NpgsqlDbType.Integer) { Value = rating_sum });
                    command.Parameters.Add(new NpgsqlParameter("@businid", NpgsqlDbType.Integer) { Value = businid });
                    try
                    {
                        // Ensure you await the execution of the command
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as needed
                        Console.WriteLine($"Error inserting data: {ex.Message}");
                    }
                   
                }
            }
        }

        public async Task ReadNormalBD()
        {
            string str = string.Empty;
            int id = default;
            List<JsonClass> js = new(); 
            await using (var connect = new NpgsqlConnection(connect_host))
            {
                connect.Open();
                await using (var command = new NpgsqlCommand("SELECT * FROM main_coworkingspaces", connect))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                        var items = new JsonClass { Id = reader.GetInt32(1), Info_cowork = reader.GetString(2), Date_created = reader.GetString(3), Date_created_snst = reader.GetString(4), Name_cowork = reader.GetString(5), Location_cowork = reader.GetString(6), Raiting_count = reader.GetInt32(7), Rating_sum = reader.GetInt32(8), Id_busin = reader.GetInt32(9)};

                        //js.Add(items);

                        if (!keyValuePairs.ContainsKey(id))
                        {
                            // If the key does not exist, create a new list for this key
                            keyValuePairs[id] = new List<JsonClass>();
                        }

                
                        keyValuePairs[id].Add(items);



                        //types.AddRange(js);
                    }

                    reader.Close();
                }
            }

        }
        public async void ReadMainWindowServicesBD()
        {
            await using (var connect = new NpgsqlConnection(cs))
            {
                connect.Open();
                await using (var command = new NpgsqlCommand("SELECT * FROM main_services", connect))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int id_coworking = reader.GetInt32(1);
                        int price = reader.GetInt32(2);
                        string str = reader.GetString(3);
                        MainServicesPairs.Add(id, (id_coworking, price, str));
                        


                    }
                    reader.Close();
                }
            }
        }
        public async void ReadServicesBd<T>(T services)
        {
            await using (var connect = new NpgsqlConnection(cs))
            {
                connect.Open();
                await using (var command = new NpgsqlCommand("SELECT * FROM main_services", connect))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int id_coworking = reader.GetInt32(1);
                        int price = reader.GetInt32(2);
                        int count = reader.GetInt32(4);
                        string str = reader.GetString(3);
                        string icons = reader.GetString(5);
                        if((services is JsonClass js) && js.Id == id_coworking || ((services is Booking book) && book.id_coworking == id_coworking ))
                        {
                            ServicesPairs.Add(id, (id_coworking, price, count, str, icons));
                        }
                    }
                    reader.Close();
                }
            }
        }
        public async Task ReadBd()
        {

            //List<object> types = [];
            string str = string.Empty;
            int id = default;
            await using (var connect = new NpgsqlConnection(connect_host))
            {
                connect.Open();
                await using (var command = new NpgsqlCommand("SELECT * FROM json_cards_info", connect))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                        str = reader.GetString(1);
                        List<JsonClass>? js = JsonConvert.DeserializeObject<List<JsonClass>>(str);
                        keyValuePairs.Add(id, js);
                        //types.AddRange(js);
                    }

                    reader.Close();
                }
            }

            //var cn = new HomePageViewModel(bd);
            //bd.DataLoaded?.Invoke(keyValuePairs);
        }
        //public async Task ReadBdCompany()
        //{
           
        //    //List<object> types = [];
      
        //    int id = default;
        //    await using (var connect = new NpgsqlConnection(connect_host))
        //    {
        //        connect.Open();
        //        await using (var command = new NpgsqlCommand("SELECT * FROM main_bookings", connect))
        //        {
        //            var reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                id = reader.GetInt32(1);
        //                var item = new IdCompany { Id_Company = id };
        //                idCompanies.Add(item);
                     
        //            }
        //            reader.Close();
        //        }
        //    }
        //}

    }

    
}
