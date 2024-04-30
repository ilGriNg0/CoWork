using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.OpenGL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using Npgsql;
using System;
using Avalonia.Interactivity;

namespace AvaloniaApplication4.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public void Login_Click(object source, RoutedEventArgs args)
        {
            var cs = "Host=localhost;Port=5432;Database=coworking;Username=postgres;Password=NoSmoking";
            var email = this.GetControl<TextBox>("Emaillog");
            var password = this.GetControl<TextBox>("Passwordlog");

            var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = $"SELECT count(*) FROM main_users WHERE email = '{email.Text}';";

            var cmd = new NpgsqlCommand(sql, con);
            var version = cmd.ExecuteScalar();

            if (version.ToString() == "0")
            {
                sql = $"SELECT count(*) FROM main_businesses WHERE email = '{email.Text}';";
                cmd = new NpgsqlCommand(sql, con);
                version = cmd.ExecuteScalar();
                if (version.ToString() == "1")
                {
                    sql = $"SELECT count(*) FROM main_businesses WHERE password = '{password.Text}' AND  email = '{email.Text}';";
                    cmd = new NpgsqlCommand(sql, con);
                    version = cmd.ExecuteScalar();
                    if (version.ToString() == "1")
                    {
                        var box = MessageBoxManager.GetMessageBoxStandard("", "Успешный вход в бизнес-аккаунт", ButtonEnum.Ok);
                        box.ShowAsync();
                        this.GetControl<TextBlock>("Error1log").IsVisible = false;
                        this.GetControl<TextBlock>("Error2log").IsVisible = false;
                    }
                    else
                    {
                        this.GetControl<TextBlock>("Error2log").IsVisible = true;
                        this.GetControl<TextBlock>("Error1log").IsVisible = false;
                    }
                }
                else
                {
                    this.GetControl<TextBlock>("Error1log").IsVisible = true;
                    this.GetControl<TextBlock>("Error2log").IsVisible = false;
                }
            }
            else
            {
                sql = $"SELECT count(*) FROM main_users WHERE password = '{password.Text}' AND  email = '{email.Text}';";
                cmd = new NpgsqlCommand(sql, con);
                version = cmd.ExecuteScalar();
                if (version.ToString() == "1")
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("", "Успешный вход в персональный аккаунт", ButtonEnum.Ok);
                    box.ShowAsync();
                    this.GetControl<TextBlock>("Error1log").IsVisible = false;
                    this.GetControl<TextBlock>("Error2log").IsVisible = false;
                }
                else
                {
                    this.GetControl<TextBlock>("Error2log").IsVisible = true;
                    this.GetControl<TextBlock>("Error1log").IsVisible = false;
                }
            }

            con.Close();
        }
    }
}
