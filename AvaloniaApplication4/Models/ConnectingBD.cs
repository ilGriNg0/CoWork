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

        public event Action<Dictionary<int, List<JsonClass>>>? DataLoaded;

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

        public async Task ReadBd()
        {
            ConnectingBD bd = new();
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
