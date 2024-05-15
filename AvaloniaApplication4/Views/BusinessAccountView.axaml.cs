using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.OpenGL;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.ViewModels;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AvaloniaApplication4.Views
{
    public partial class BusinessAccountView : UserControl
    {
        public SolidColorBrush newbrush = new(Colors.Red);
        public SolidColorBrush lastbrush = new(Colors.Black);
        public SolidColorBrush oldbrush = new(Colors.White);

        public BusinessAccountView()
        {
            InitializeComponent();
        }

        private void Exit_Click(object source, RoutedEventArgs args)
        {
            User.Model = null;
            User.Main.Page = new LoginViewModel();
        }

        private void Change_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (!this.GetControl<TextBox>("Emailbus").IsEnabled)
            {
                this.GetControl<TextBox>("Emailbus").IsEnabled = true;
                this.GetControl<TextBox>("Phonebus").IsEnabled = true;
                this.GetControl<TextBox>("Passwordbus").IsEnabled = true;

                this.GetControl<TextBox>("Emailbus").Foreground = lastbrush;
                this.GetControl<TextBox>("Phonebus").Foreground = lastbrush;
                this.GetControl<TextBox>("Passwordbus").Foreground = lastbrush;

                this.GetControl<TextBlock>("ChangeBlock").Text = "������";

                this.GetControl<TextBlock>("SavingBlock").IsVisible = true;
            }
            else
            {
                this.GetControl<TextBox>("Emailbus").IsEnabled = false;
                this.GetControl<TextBox>("Phonebus").IsEnabled = false;
                this.GetControl<TextBox>("Passwordbus").IsEnabled = false;

                this.GetControl<TextBox>("Emailbus").Foreground = oldbrush;
                this.GetControl<TextBox>("Phonebus").Foreground = oldbrush;
                this.GetControl<TextBox>("Passwordbus").Foreground = oldbrush;

                this.GetControl<TextBlock>("ChangeBlock").Text = "�������� ������";
                this.GetControl<TextBlock>("SavingBlock").IsVisible = false;
                this.GetControl<TextBlock>("Error").IsVisible = false;

                this.GetControl<TextBox>("Emailbus").Text = User.Email;
                this.GetControl<TextBox>("Phonebus").Text = User.Phone;
                this.GetControl<TextBox>("Passwordbus").Text = User.Password;
            }
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            switch (textBox.Name)
            {
                case "Phonebus":
                    if (MyRegex().Matches(textBox.Text).Count != 0) { textBox.BorderBrush = newbrush; }
                    else { textBox.BorderBrush = lastbrush; }
                    break;

                case "Emailbus":
                    if (textBox.Text == null || textBox.Text == "" || !CheckEmail(textBox.Text)) { textBox.BorderBrush = newbrush; }
                    else { textBox.BorderBrush = lastbrush; }
                    break;

                case "Passwordbus":
                    if (textBox.Text == null || textBox.Text.Length < 8) { textBox.BorderBrush = newbrush; }
                    else { textBox.BorderBrush = lastbrush; }
                    break;
            }
        }

        public static bool CheckEmail(string email)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new(strRegex);
            if (re.IsMatch(email))
                return true;
            else
                return false;
        }

        [GeneratedRegex("_")]
        private static partial Regex MyRegex();

        private void Save_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (this.GetControl<TextBox>("Emailbus").BorderBrush != newbrush && this.GetControl<TextBox>("Phonebus").BorderBrush != newbrush && this.GetControl<TextBox>("Passwordbus").BorderBrush != newbrush)
                if (this.GetControl<TextBox>("Emailbus").Text != User.Email || this.GetControl<TextBox>("Phonebus").Text != User.Phone || this.GetControl<TextBox>("Passwordbus").Text != User.Password) 
                {
                    var cs = "Host=localhost;Port=5432;Database=coworking;Username=postgres;Password=NoSmoking";
                    var con = new NpgsqlConnection(cs);
                    con.Open();

                    var sql1 = $"SELECT count(*) FROM main_users WHERE email = '{this.GetControl<TextBox>("Emailbus").Text.ToLower()}';";
                    var cmd1 = new NpgsqlCommand(sql1, con);
                    var version1 = cmd1.ExecuteScalar();
                    var sql2 = $"SELECT count(*) FROM main_businesses WHERE email = '{this.GetControl<TextBox>("Emailbus").Text.ToLower()}';";
                    var cmd2 = new NpgsqlCommand(sql2, con);
                    var version2 = cmd2.ExecuteScalar();

                    if (version1.ToString() != version2.ToString() && this.GetControl<TextBox>("Emailbus").Text != User.Email)
                    {
                        Error.IsVisible = true;
                        return;
                    }

                    sql1 = $"UPDATE main_businesses SET email = '{this.GetControl<TextBox>("Emailbus").Text.ToLower()}', phone_number = '{this.GetControl<TextBox>("Phonebus").Text.Split("�� ")[1]}', password = '{this.GetControl<TextBox>("Passwordbus").Text}' WHERE id = {User.Id};";

                    User.Email = this.GetControl<TextBox>("Emailbus").Text;
                    User.Password = this.GetControl<TextBox>("Passwordbus").Text;
                    User.Phone = this.GetControl<TextBox>("Phonebus").Text.Split("�� ")[1].Split("+7")[1].Replace("-", "");

                    cmd1 = new NpgsqlCommand(sql1, con);
                    cmd1.ExecuteScalar();

                    this.GetControl<TextBox>("Emailbus").IsEnabled = false;
                    this.GetControl<TextBox>("Phonebus").IsEnabled = false;
                    this.GetControl<TextBox>("Passwordbus").IsEnabled = false;

                    this.GetControl<TextBox>("Emailbus").Foreground = oldbrush;
                    this.GetControl<TextBox>("Phonebus").Foreground = oldbrush;
                    this.GetControl<TextBox>("Passwordbus").Foreground = oldbrush;

                    this.GetControl<TextBlock>("ChangeBlock").Text = "�������� ������";
                    this.GetControl<TextBlock>("SavingBlock").IsVisible = false;
                    this.GetControl<TextBlock>("Error").IsVisible = false;
                }
        }
    }
}
