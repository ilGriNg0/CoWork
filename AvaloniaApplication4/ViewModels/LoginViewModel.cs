using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Npgsql;
using System;
using System.Diagnostics.Tracing;
namespace AvaloniaApplication4.ViewModels
{
    public partial class LoginViewModel : ViewModelBase 
    {
        [ObservableProperty] private String _email;
        [ObservableProperty] private String _password;

        [RelayCommand]
        public void Login()
        {
            var cs = "Host=localhost;Port=5432;Database=coworking;Username=postgres;Password=NoSmoking";

            var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = $"SELECT count(*) FROM users WHERE password = '{Password}' AND email = '{Email}'";

            var cmd = new NpgsqlCommand(sql, con);
            var version = cmd.ExecuteScalar();

            if (version is null) return;

            if (version.ToString() == "1")
            {
                //System.Diagnostics.Debug.WriteLine($"\nPostgreSQL version: {version.GetString(1)}");
                Email = "good";
            }
            else Email = "bad";

            con.Close();
        }
    }
}
