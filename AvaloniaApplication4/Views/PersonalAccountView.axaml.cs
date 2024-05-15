using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.ViewModels;
using Npgsql;
using System.Text.RegularExpressions;

namespace AvaloniaApplication4.Views
{
    public partial class PersonalAccountView : UserControl
    {
        public SolidColorBrush newbrush = new(Colors.Red);
        public SolidColorBrush lastbrush = new(Colors.Black);
        public SolidColorBrush oldbrush = new(Colors.White);
        public PersonalAccountView()
        {
            InitializeComponent();
        }

        private void Exit_Click(object source, RoutedEventArgs args)
        {
            User.Model = null;
            User.Main.CurrentPage = new LoginViewModel();
        }

        private void Change_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (!this.GetControl<TextBox>("EmailBox").IsEnabled)
            {
                this.GetControl<TextBox>("EmailBox").IsEnabled = true;
                this.GetControl<TextBox>("PhoneBox").IsEnabled = true;
                this.GetControl<TextBox>("PasswordBox").IsEnabled = true;

                this.GetControl<TextBox>("EmailBox").Foreground = lastbrush;
                this.GetControl<TextBox>("PhoneBox").Foreground = lastbrush;
                this.GetControl<TextBox>("PasswordBox").Foreground = lastbrush;

                this.GetControl<TextBlock>("ChangeBlock").Text = "Отмена";

                this.GetControl<TextBlock>("SavingBlock").IsVisible = true;
            }
            else
            {
                this.GetControl<TextBox>("EmailBox").IsEnabled = false;
                this.GetControl<TextBox>("PhoneBox").IsEnabled = false;
                this.GetControl<TextBox>("PasswordBox").IsEnabled = false;

                this.GetControl<TextBox>("EmailBox").Foreground = oldbrush;
                this.GetControl<TextBox>("PhoneBox").Foreground = oldbrush;
                this.GetControl<TextBox>("PasswordBox").Foreground = oldbrush;

                this.GetControl<TextBlock>("ChangeBlock").Text = "Изменить данные";
                this.GetControl<TextBlock>("SavingBlock").IsVisible = false;
                this.GetControl<TextBlock>("Error").IsVisible = false;

                this.GetControl<TextBox>("EmailBox").Text = User.Email;
                this.GetControl<TextBox>("PhoneBox").Text = User.Phone;
                this.GetControl<TextBox>("PasswordBox").Text = User.Password;
            }
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            switch (textBox.Name)
            {
                case "PhoneBox":
                    if (MyRegex().Matches(textBox.Text).Count != 0) { textBox.BorderBrush = newbrush; }
                    else { textBox.BorderBrush = lastbrush; }
                    break;

                case "EmailBox":
                    if (textBox.Text == null || textBox.Text == "" || !CheckEmail(textBox.Text)) { textBox.BorderBrush = newbrush; }
                    else { textBox.BorderBrush = lastbrush; }
                    break;

                case "PasswordBox":
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
            if (this.GetControl<TextBox>("EmailBox").BorderBrush != newbrush && this.GetControl<TextBox>("PhoneBox").BorderBrush != newbrush && this.GetControl<TextBox>("PasswordBox").BorderBrush != newbrush)
                if (this.GetControl<TextBox>("EmailBox").Text != User.Email || this.GetControl<TextBox>("PhoneBox").Text != User.Phone || this.GetControl<TextBox>("PasswordBox").Text != User.Password)
                {
                    var cs = "Host=localhost;Port=5432;Database=coworking;Username=postgres;Password=NoSmoking";

                    var con = new NpgsqlConnection(cs);
                    con.Open();
                    var sql1 = $"SELECT count(*) FROM main_users WHERE email = '{this.GetControl<TextBox>("EmailBox").Text.ToLower()}';";
                    var cmd1 = new NpgsqlCommand(sql1, con);
                    var version1 = cmd1.ExecuteScalar();
                    var sql2 = $"SELECT count(*) FROM main_businesses WHERE email = '{this.GetControl<TextBox>("EmailBox").Text.ToLower()}';";
                    var cmd2 = new NpgsqlCommand(sql2, con);
                    var version2 = cmd2.ExecuteScalar();
                    if (version1.ToString() != version2.ToString() && this.GetControl<TextBox>("EmailBox").Text != User.Email)
                    {
                        this.GetControl<TextBlock>("Error").IsVisible = true;
                        return;
                    }


                    sql1 = $"UPDATE main_users SET email = '{this.GetControl<TextBox>("EmailBox").Text.ToLower()}', phone_number = '{this.GetControl<TextBox>("PhoneBox").Text.Split("он ")[1]}', password = '{this.GetControl<TextBox>("PasswordBox").Text}' WHERE id = {User.Id};";

                    User.Email = this.GetControl<TextBox>("EmailBox").Text;
                    User.Password = this.GetControl<TextBox>("PasswordBox").Text;
                    User.Phone = this.GetControl<TextBox>("PhoneBox").Text.Split("он ")[1].Split("+7")[1].Replace("-", "");

                    cmd1 = new NpgsqlCommand(sql1, con);
                    cmd1.ExecuteScalar();

                    this.GetControl<TextBox>("EmailBox").IsEnabled = false;
                    this.GetControl<TextBox>("PhoneBox").IsEnabled = false;
                    this.GetControl<TextBox>("PasswordBox").IsEnabled = false;

                    this.GetControl<TextBox>("EmailBox").Foreground = oldbrush;
                    this.GetControl<TextBox>("PhoneBox").Foreground = oldbrush;
                    this.GetControl<TextBox>("PasswordBox").Foreground = oldbrush;

                    this.GetControl<TextBlock>("ChangeBlock").Text = "Изменить данные";
                    this.GetControl<TextBlock>("SavingBlock").IsVisible = false;
                    this.GetControl<TextBlock>("Error").IsVisible = false;
                }
        }
    }
}
