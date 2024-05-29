using AvaloniaApplication4.ViewModels;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        public Dictionary<(int, int), string> PhotoIDPathPairs = new();
        public Dictionary<(int, int), string> PhotoIDPathBusinessPairs = new();
        public Dictionary<int, (int, int, int, string, string)> ServicesPairs = new();

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
                string command_add = "INSERT INTO main_images (id_coworking_id, img) VALUES (@intValue, @text)";
                await using (var command = new NpgsqlCommand(command_add, connect))
                {
                    command.Parameters.Add(new NpgsqlParameter("@intValue", NpgsqlDbType.Integer) { Value = id });
                    command.Parameters.Add(new NpgsqlParameter("@text", NpgsqlDbType.Text) { Value = path });
                    command.ExecuteNonQuery();
                }
            }
        }
        public async Task WriteServicesBd(int id, int price1, int count1, string info, string icon, int price2, int count2)
        {
            await using (var conncet = new NpgsqlConnection(connect_host))
            {
                conncet.Open();
                string command_add_serv = "INSERT INTO main_services (id_coworking_id, price, count, type, img) VALUES (@id, @price1, @count1, @info, @icon);" +
                                          "INSERT INTO main_services (id_coworking_id, price, count, type, img) VALUES (@id, @price2, @count2, '', '')";
                await using (var command = new NpgsqlCommand(command_add_serv, conncet))
                {
                    command.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Integer) { Value = id });
                    command.Parameters.Add(new NpgsqlParameter("@price1", NpgsqlDbType.Integer) { Value = price1 });
                    command.Parameters.Add(new NpgsqlParameter("@count1", NpgsqlDbType.Integer) { Value = count1 });
                    command.Parameters.Add(new NpgsqlParameter("@info", NpgsqlDbType.Varchar) { Value = info });
                    command.Parameters.Add(new NpgsqlParameter("@icon", NpgsqlDbType.Varchar) { Value = icon });
                    command.Parameters.Add(new NpgsqlParameter("@price2", NpgsqlDbType.Integer) { Value = price2 });
                    command.Parameters.Add(new NpgsqlParameter("@count2", NpgsqlDbType.Integer) { Value = count2 });
                    command.ExecuteNonQuery();
                }
            }
        }
        public async Task WriteNormalBD(int id_company, string info, string  time_start, string time_end, string name, string address, int rating_ctn, int rating_sum)
        {
            await using (var conncet = new NpgsqlConnection(connect_host))
            {
                conncet.Open();
                string command_add_serv = "INSERT INTO main_coworkingspaces (id_company_id, description, time_start, time_end, coworking_name, address, rating_count, rating_sum) VALUES (@id_company, @info, @time_start, @time_end, @name, @address, @rating_ctn, @rating_sum);";
                await using (var command = new NpgsqlCommand(command_add_serv, conncet))
                {
                    command.Parameters.Add(new NpgsqlParameter("@id_company", NpgsqlDbType.Integer) { Value = id_company });
                    command.Parameters.Add(new NpgsqlParameter("@info", NpgsqlDbType.Varchar) { Value = info });
                    command.Parameters.Add(new NpgsqlParameter("@time_start", NpgsqlDbType.Varchar) { Value = time_start });
                    command.Parameters.Add(new NpgsqlParameter("@time_end", NpgsqlDbType.Varchar) { Value = time_end });
                    command.Parameters.Add(new NpgsqlParameter("@name", NpgsqlDbType.Varchar) { Value = name });
                    command.Parameters.Add(new NpgsqlParameter("@address", NpgsqlDbType.Varchar) { Value = address });
                    command.Parameters.Add(new NpgsqlParameter("@rating_ctn", NpgsqlDbType.Integer) { Value = rating_ctn });
                    command.Parameters.Add(new NpgsqlParameter("@rating_sum", NpgsqlDbType.Integer) { Value = rating_sum });
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task ReadNormalBD()
        {
            string str = string.Empty;
            int id = default;
            await using (var connect = new NpgsqlConnection(connect_host))
            {
                connect.Open();
                await using (var command = new NpgsqlCommand("SELECT * FROM main_coworkingspaces", connect))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var items = new JsonClass { Id = reader.GetInt32(1), Info_cowork = reader.GetString(2), Date_created = reader.GetString(3), Date_created_snst = reader.GetString(4), Name_cowork = reader.GetString(5), Location_cowork = reader.GetString(6), Raiting_count = reader.GetInt32(7), Rating_sum = reader.GetInt32(8)};
                        List<JsonClass>? js = [items];
                        keyValuePairs.Add(id, js);
                        //types.AddRange(js);
                    }

                    reader.Close();
                }
            }

        }
        public async void ReadServicesBd()
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
                        ServicesPairs.Add(id, (id_coworking, price, count, str, icons));
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
