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
            var cs = User.Connect;

            if (this.GetControl<TextBox>("Passwordlog").Text == null || this.GetControl<TextBox>("Passwordlog").Text.Equals("") || this.GetControl<TextBox>("Emaillog").Text == null || this.GetControl<TextBox>("Emaillog").Text.Equals("")) return;
            var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = $"SELECT count(*) FROM main_users WHERE email = '{this.GetControl<TextBox>("Emaillog").Text.ToLower()}';";

            var cmd = new NpgsqlCommand(sql, con);
            var version = cmd.ExecuteScalar();

            if (version.ToString() == "0")
            {
                sql = $"SELECT count(*) FROM main_businesses WHERE email = '{this.GetControl<TextBox>("Emaillog").Text.ToLower()}';";
                cmd = new NpgsqlCommand(sql, con);
                version = cmd.ExecuteScalar();
                var instance_busin = (BusinessAccountViewModel)Activator.CreateInstance(typeof(BusinessAccountViewModel), true);
                instance_busin.KeyBusin = true;
                if (version.ToString() == "1")
                {
                    sql = $"SELECT id FROM main_businesses WHERE password = '{this.GetControl<TextBox>("Passwordlog").Text}' AND email = '{this.GetControl<TextBox>("Emaillog").Text.ToLower()}';";
                    cmd = new NpgsqlCommand(sql, con);
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        this.GetControl<TextBlock>("Error1log").IsVisible = false;
                        this.GetControl<TextBlock>("Error2log").IsVisible = false;

                        User.Id = rdr.GetInt64(0); 
                        BusinessAccountViewModel.IdBusinUser = (int)User.Id;
                        User.Model = new BusinessAccountViewModel();
                        User.Main.Page = User.Model;
                        return;
                    }
                    this.GetControl<TextBlock>("Error2log").IsVisible = true;
                    this.GetControl<TextBlock>("Error1log").IsVisible = false;
                }
                else
                {
                    this.GetControl<TextBlock>("Error1log").IsVisible = true;
                    this.GetControl<TextBlock>("Error2log").IsVisible = false;
                }
            }
            else
            {
                sql = $"SELECT id FROM main_users WHERE password = '{this.GetControl<TextBox>("Passwordlog").Text}' AND email = '{this.GetControl<TextBox>("Emaillog").Text.ToLower()}';";
                cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                var myClassType = typeof(LoginViewModel);
                var instance = (LoginViewModel)Activator.CreateInstance(myClassType, true);
                instance.IsReg = true;
                while (rdr.Read())
                {
                    this.GetControl<TextBlock>("Error1log").IsVisible = false;
                    this.GetControl<TextBlock>("Error2log").IsVisible = false;

                    User.Id = rdr.GetInt64(0);
                    User.Model = new PersonalAccountViewModel();
                    User.Main.Page = User.Model;
                    return;
                }
                this.GetControl<TextBlock>("Error2log").IsVisible = true;
                this.GetControl<TextBlock>("Error1log").IsVisible = false;
               
            }
            con.Close();
           
        }
    }
}
