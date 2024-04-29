using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
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

            var sql = $"SELECT count(*) FROM main_users WHERE password = '{Password}' AND email = '{Email}';";

            var cmd = new NpgsqlCommand(sql, con);
            var version = cmd.ExecuteScalar();

            if (version.ToString() == "0")
            {
                sql = $"SELECT count(*) FROM main_businesses WHERE password = '{Password}' AND email = '{Email}';";
                cmd = new NpgsqlCommand(sql, con);
                version = cmd.ExecuteScalar();
                if (version.ToString() == "1")
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("", "Успешный вход в бизнес-аккаунт", ButtonEnum.Ok);
                    var result = box.ShowAsync();
                }
            }
            else 
            {
                var box = MessageBoxManager.GetMessageBoxStandard("", "Успешный вход в персональный аккаунт", ButtonEnum.Ok);
                var result = box.ShowAsync();
            }

            con.Close();
        }
    }
}
