﻿using AvaloniaApplication4.ViewModels;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication4.Models
{
    public partial class ConnectingBD
    {
        string connect_host = "Host=localhost;Port=5432;Database=cardtest;Username=postgres;Password=123456";
        public Dictionary<int, List<JsonClass>> keyValuePairs { get; set; } = new Dictionary<int, List<JsonClass>>();

        public event Action<Dictionary<int, List<JsonClass>>>? DataLoaded;




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

    }
}