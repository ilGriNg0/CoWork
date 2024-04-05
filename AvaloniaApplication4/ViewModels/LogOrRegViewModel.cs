using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Npgsql;
using System;
namespace AvaloniaApplication4.ViewModels
{
    public partial class LogOrRegViewModel : ViewModelBase 
    {
        [RelayCommand]
        public void Click()
        {
            var cs = "Host=localhost;Port=5432;Username=postgres;Password=NoSmoking;Database=coworking";

            var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT version()";

            var cmd = new NpgsqlCommand(sql, con);

            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"\nPostgreSQL version: {version}");
        }
    }
}
