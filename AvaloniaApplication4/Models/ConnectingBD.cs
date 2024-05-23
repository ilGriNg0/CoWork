using AvaloniaApplication4.ViewModels;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication4.Models
{
    public partial class ConnectingBD
    {
        string connect_host = User.Connect;
        public Dictionary<int, List<JsonClass>> keyValuePairs { get; set; } = new Dictionary<int, List<JsonClass>>();
        public Dictionary<(int, int), string> PhotoIDPathPairs = new();
        public Dictionary<(int, int), string> PhotoIDPathBusinessPairs = new();
      

        public ObservableCollection<IdCompany> idCompanies { get; set; } = new ObservableCollection<IdCompany>();

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
        public string cs = User.Connect;
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
        public async Task WriteBusinessBd(string path, int id)
        {
           
            await using (var connect = new NpgsqlConnection(connect_host))
            {
                connect.Open();
                string command_add = "INSERT INTO main_images (id_coworking, file) VALUES (@intValue, @text)";
                await using (var command = new NpgsqlCommand(command_add, connect))
                {
                    command.Parameters.Add(new NpgsqlParameter("@intValue", NpgsqlDbType.Integer) {Value = id });
                    command.Parameters.Add(new NpgsqlParameter("@text", NpgsqlDbType.Text) { Value = path });
                    command.ExecuteNonQuery();
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
        public async Task ReadBdCompany()
        {
           
            //List<object> types = [];
      
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

    }
}
