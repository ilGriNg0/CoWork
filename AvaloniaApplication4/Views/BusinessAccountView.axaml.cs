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
            if (!Emailbus.IsEnabled)
            {
                Emailbus.IsEnabled = true;
                Phonebus.IsEnabled = true;
                Passwordbus.IsEnabled = true;

                Emailbus.Foreground = lastbrush;
                Phonebus.Foreground = lastbrush;
                Passwordbus.Foreground = lastbrush;

                ChangeBlock.Text = "Отмена";

                SavingBlock.IsVisible = true;
            }
            else
            {
                Emailbus.IsEnabled = false;
                Phonebus.IsEnabled = false;
                Passwordbus.IsEnabled = false;

                Emailbus.Foreground = oldbrush;
                Phonebus.Foreground = oldbrush;
                Passwordbus.Foreground = oldbrush;

                ChangeBlock.Text = "Изменить данные";
                SavingBlock.IsVisible = false;
                Error.IsVisible = false;

                Emailbus.Text = User.Email;
                Phonebus.Text = User.Phone;
                Passwordbus.Text = User.Password;
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
            if (Emailbus.BorderBrush != newbrush && Phonebus.BorderBrush != newbrush && Passwordbus.BorderBrush != newbrush)
                if (Emailbus.Text != User.Email || Phonebus.Text != User.Phone || Passwordbus.Text != User.Password) 
                {
                    var cs = "Host=localhost;Port=5432;Database=coworking;Username=postgres;Password=NoSmoking";
                    var con = new NpgsqlConnection(cs);
                    con.Open();

                    var sql1 = $"SELECT count(*) FROM main_users WHERE email = '{Emailbus.Text.ToLower()}';";
                    var cmd1 = new NpgsqlCommand(sql1, con);
                    var version1 = cmd1.ExecuteScalar();
                    var sql2 = $"SELECT count(*) FROM main_businesses WHERE email = '{Emailbus.Text.ToLower()}';";
                    var cmd2 = new NpgsqlCommand(sql2, con);
                    var version2 = cmd2.ExecuteScalar();

                    if (version1.ToString() != version2.ToString() && Emailbus.Text != User.Email)
                    {
                        Error.IsVisible = true;
                        return;
                    }

                    sql1 = $"UPDATE main_businesses SET email = '{Emailbus.Text.ToLower()}', phone_number = '{Phonebus.Text.Split("ка ")[1]}', password = '{Passwordbus.Text}' WHERE id = {User.Id};";

                    User.Email = Emailbus.Text;
                    User.Password = Passwordbus.Text;
                    User.Phone = Phonebus.Text.Split("ка ")[1].Split("+7")[1].Replace("-", "");

                    cmd1 = new NpgsqlCommand(sql1, con);
                    cmd1.ExecuteScalar();

                    Emailbus.IsEnabled = false;
                    Phonebus.IsEnabled = false;
                    Passwordbus.IsEnabled = false;

                    Emailbus.Foreground = oldbrush;
                    Phonebus.Foreground = oldbrush;
                    Passwordbus.Foreground = oldbrush;

                    ChangeBlock.Text = "Изменить данные";
                    SavingBlock.IsVisible = false;
                    Error.IsVisible = false;
                }
        }
    }
}
