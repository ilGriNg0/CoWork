using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.OpenGL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Npgsql;
using System;
using Avalonia.Interactivity;
using AvaloniaApplication4.ViewModels;
using AvaloniaApplication4.Models;

namespace AvaloniaApplication4.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Forgot_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            return;
        }

        private void NoAccount_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            User.Model = new RegistrationViewModel();
            User.Main.Page = User.Model;
        }

        public void Login_Click(object source, RoutedEventArgs args)
        {
            var cs = "Host=localhost;Port=5432;Database=coworking;Username=postgres;Password=NoSmoking";

            if (Passwordlog.Text == null || Passwordlog.Text.Equals("") || Emaillog.Text == null || Emaillog.Text.Equals("")) return;
            var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = $"SELECT count(*) FROM main_users WHERE email = '{Emaillog.Text.ToLower()}';";

            var cmd = new NpgsqlCommand(sql, con);
            var version = cmd.ExecuteScalar();

            if (version.ToString() == "0")
            {
                sql = $"SELECT count(*) FROM main_businesses WHERE email = '{Emaillog.Text.ToLower()}';";
                cmd = new NpgsqlCommand(sql, con);
                version = cmd.ExecuteScalar();
                if (version.ToString() == "1")
                {
                    sql = $"SELECT count(*) FROM main_businesses WHERE password = '{Passwordlog.Text}' AND email = '{Emaillog.Text.ToLower()}';";
                    cmd = new NpgsqlCommand(sql, con);
                    version = cmd.ExecuteScalar();
                    if (version.ToString() == "1")
                    {
                        Error1log.IsVisible = false;
                        Error2log.IsVisible = false;

                        sql = $"SELECT id FROM main_businesses WHERE email = '{Emaillog.Text.ToLower()}';";
                        cmd = new NpgsqlCommand(sql, con);
                        version = cmd.ExecuteScalar();
                        User.Id = Int64.Parse(version.ToString());
                        User.Model = new BusinessAccountViewModel();
                        User.Main.Page = User.Model;

                        
                    }
                    else
                    {
                        Error2log.IsVisible = true;
                        Error1log.IsVisible = false;
                    }
                }
                else
                {
                    Error1log.IsVisible = true;
                    Error2log.IsVisible = false;
                }
            }
            else
            {
                sql = $"SELECT count(*) FROM main_users WHERE password = '{Passwordlog.Text}' AND email = '{Emaillog.Text.ToLower()}';";
                cmd = new NpgsqlCommand(sql, con);
                version = cmd.ExecuteScalar();
                if (version.ToString() == "1")
                {
                    Error1log.IsVisible = false;
                    Error2log.IsVisible = false;

                    sql = $"SELECT id FROM main_users WHERE email = '{Emaillog.Text.ToLower()}';";
                    cmd = new NpgsqlCommand(sql, con);
                    version = cmd.ExecuteScalar();
                    User.Id = Int64.Parse(version.ToString());
                    User.Model = new PersonalAccountViewModel();
                    User.Main.Page = User.Model;

                  
                }
                else
                {
                    Error2log.IsVisible = true;
                    Error1log.IsVisible = false;
                }
            }

            con.Close();
        }
    }
}
