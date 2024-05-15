using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Converters;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Xml.Schema;
using Avalonia.Media;
using System.Net.Sockets;
using System.Net;
using Tmds.DBus.Protocol;
using Npgsql;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Avalonia.Input;
using AvaloniaApplication4.ViewModels;
using AvaloniaApplication4.Models;

namespace AvaloniaApplication4.Views
{
    public partial class RegistrationView : UserControl
    {
        public SolidColorBrush newbrush = new(Colors.Red);
        public SolidColorBrush lastbrush = new(Colors.Black);

        public bool phoneper = false;
        public bool dateper = false;
        public bool phonebus = false;

        public int person = 127;
        public int business = 31;

        public RegistrationView()
        {
            InitializeComponent();
        }

        private void HasAccount_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            User.Model = new LoginViewModel();
            User.Main.Page = User.Model;
        }

        static int SetBit(int num, int nbit, int bit)
        {
            int mask = (1 << nbit);

            if (bit == 1)
                num |= mask;
            else
                num = (num | mask) ^ mask;

            return num;
        }

        public void Find_Click(object source, RoutedEventArgs args)
        {   
            if (this.GetControl<Border>("spaceman1").IsVisible)
            {
                return;
            }
            else if (this.GetControl<Border>("woodcutter1").IsVisible)
            {
                this.GetControl<Border>("woodcutter1").IsVisible = false;
                this.GetControl<Border>("woodcutter").IsVisible = true;
            }
            this.GetControl<Border>("spaceman1").IsVisible = true;
            this.GetControl<Border>("spaceman").IsVisible = false;
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            switch(textBox.Name)
            {
                case "Firstper":
                    if (textBox.Text == null || textBox.Text == "") { textBox.BorderBrush = newbrush; person = SetBit(person, 0, 1); }
                        else { textBox.BorderBrush = lastbrush; person = SetBit(person, 0, 0); }
                    break;

                case "Lastper":
                    if (textBox.Text == null || textBox.Text == "") { textBox.BorderBrush = newbrush; person = SetBit(person, 1, 1); }
                        else { textBox.BorderBrush = lastbrush; person = SetBit(person, 1, 0); }
                    break;

                case "Emailper":
                    if (textBox.Text == null || textBox.Text == "" || !CheckEmail(textBox.Text)) { textBox.BorderBrush = newbrush; person = SetBit(person, 2, 1); }
                        else { textBox.BorderBrush = lastbrush; person = SetBit(person, 2, 0); }
                    break;

                case "Phoneper":
                    if (phoneper && MyRegex().Matches(textBox.Text).Count != 0) { textBox.BorderBrush = newbrush; person = SetBit(person, 3, 1); }
                        else { textBox.BorderBrush = lastbrush; phoneper = true; person = SetBit(person, 3, 0); }
                    break;

                
                case "Dateper":
                    if (dateper && (MyRegex().Matches(textBox.Text).Count != 0 || !CheckDate(textBox.Text.Split("ия ")[1]))) { textBox.BorderBrush = newbrush; person = SetBit(person, 4, 1); }
                        else { textBox.BorderBrush = lastbrush; dateper = true; person = SetBit(person, 4, 0); }
                    break;

                case "Password1per":
                    if (textBox.Text == null || textBox.Text.Length < 8) { textBox.BorderBrush = newbrush; person = SetBit(person, 5, 1); }
                        else { textBox.BorderBrush = lastbrush; person = SetBit(person, 5, 0); }
                    if (this.GetControl<TextBox>("Password2per").Text != null && !textBox.Text.Equals(this.GetControl<TextBox>("Password2per").Text)) { this.GetControl<TextBox>("Password2per").BorderBrush = newbrush; person = SetBit(person, 6, 1); }
                        else { this.GetControl<TextBox>("Password2per").BorderBrush = lastbrush; person = SetBit(person, 6, 0); }
                    break;

                case "Password2per":
                    if (!textBox.Text.Equals(this.GetControl<TextBox>("Password1per").Text)) { textBox.BorderBrush = newbrush; person = SetBit(person, 6, 1); }
                        else { textBox.BorderBrush = lastbrush; person = SetBit(person, 6, 0); }
                    break;


                case "Namebus":
                    if (textBox.Text == null || textBox.Text == "") { textBox.BorderBrush = newbrush; business = SetBit(business, 0, 1); }
                        else { textBox.BorderBrush = lastbrush; business = SetBit(business, 0, 0); }
                    break;

                case "Phonebus":
                    if (phonebus && MyRegex().Matches(textBox.Text).Count != 0) { textBox.BorderBrush = newbrush; business = SetBit(business, 1, 1); }
                        else { textBox.BorderBrush = lastbrush; phonebus = true; business = SetBit(business, 1, 0); }
                    break;

                case "Emailbus":
                    if (textBox.Text == null || textBox.Text == "" || !CheckEmail(textBox.Text)) { textBox.BorderBrush = newbrush; business = SetBit(business, 2, 1); }
                        else { textBox.BorderBrush = lastbrush; business = SetBit(business, 2, 0); }
                    break;

                case "Password1bus":
                    if (textBox.Text == null || textBox.Text.Length < 8) { textBox.BorderBrush = newbrush; business = SetBit(business, 3, 1); }
                        else { textBox.BorderBrush = lastbrush; business = SetBit(business, 3, 0); }
                    if (this.GetControl<TextBox>("Password2bus").Text != null && !textBox.Text.Equals(this.GetControl<TextBox>("Password2bus").Text)) { this.GetControl<TextBox>("Password2bus").BorderBrush = newbrush; business = SetBit(business, 4, 1); }
                        else { this.GetControl<TextBox>("Password2bus").BorderBrush = lastbrush; business = SetBit(business, 4, 0); }
                    break;

                case "Password2bus":
                    if (!textBox.Text.Equals(this.GetControl<TextBox>("Password1bus").Text)) { textBox.BorderBrush = newbrush; business = SetBit(business, 4, 1); }
                        else { textBox.BorderBrush = lastbrush; business = SetBit(business, 4, 0); }
                    break;
            }
            
        }
        public void Find_all_Click(object source, RoutedEventArgs args)
        {
            if (person == 0)
            {
                var cs = "Host=localhost;Port=5432;Database=coworking;Username=postgres;Password=NoSmoking";

                var con = new NpgsqlConnection(cs);
                con.Open();
                var sql = $"SELECT count(*) FROM main_users WHERE email = '{this.GetControl<TextBox>("Emailper").Text.ToLower()}';";
                var cmd = new NpgsqlCommand(sql, con);
                var version = cmd.ExecuteScalar();

                if (version.ToString() == "0")
                {
                    sql = $"SELECT count(*) FROM main_businesses WHERE email = '{this.GetControl<TextBox>("Emailper").Text.ToLower()}';";
                    cmd = new NpgsqlCommand(sql, con);
                    version = cmd.ExecuteScalar();

                    if (version.ToString() == "0")
                    {
                        sql = $"INSERT INTO main_users(email, password, first_name, last_name, phone_number, date_of_birth) " +
                            $"VALUES ('{this.GetControl<TextBox>("Emailper").Text.ToLower()}', '{this.GetControl<TextBox>("Password1per").Text}', '{this.GetControl<TextBox>("Firstper").Text}', '{this.GetControl<TextBox>("Lastper").Text}', " +
                            $"'{this.GetControl<TextBox>("Phoneper").Text.Split("он ")[1]}', '{DateTime.Parse(this.GetControl<TextBox>("Dateper").Text.Split("ия ")[1]).ToShortDateString()}');";
                        cmd = new NpgsqlCommand(sql, con);
                        cmd.ExecuteScalar();
                        this.GetControl<TextBlock>("Errorper").IsVisible = false;

                        sql = $"SELECT id FROM main_users WHERE email = '{this.GetControl<TextBox>("Emailper").Text.ToLower()}';";
                        cmd = new NpgsqlCommand(sql, con);
                        version = cmd.ExecuteScalar();
                        User.Id = Int64.Parse(version.ToString());
                        User.Model = new PersonalAccountViewModel();
                        User.Main.Page = User.Model;
                    }
                    else
                    {
                        this.GetControl<TextBlock>("Errorper").IsVisible = true;
                    }
                }
                else
                {
                    this.GetControl<TextBlock>("Errorper").IsVisible = true;
                }
                con.Close();
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

        public static bool CheckDate(string date)
        {
            try
            {
                if (DateTime.Now.Year - DateTime.Parse(date).Year > 14 && DateTime.Now.Year - DateTime.Parse(date).Year < 100) return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public void Create_all_Click(object source, RoutedEventArgs args)
        {
            if (business == 0)
            {
                var cs = "Host=localhost;Port=5432;Database=coworking;Username=postgres;Password=NoSmoking";

                var con = new NpgsqlConnection(cs);
                con.Open();
                var sql = $"SELECT count(*) FROM main_businesses WHERE email = '{this.GetControl<TextBox>("Emailbus").Text.ToLower()}';";
                var cmd = new NpgsqlCommand(sql, con);
                var version = cmd.ExecuteScalar();

                if (version.ToString() == "0")
                {
                    sql = $"SELECT count(*) FROM main_users WHERE email = '{this.GetControl<TextBox>("Emailbus").Text.ToLower()}';";
                    cmd = new NpgsqlCommand(sql, con);
                    version = cmd.ExecuteScalar();

                    if (version.ToString() == "0")
                    {
                        sql = $"INSERT INTO main_businesses(email, password, company_name, phone_number) " +
                            $"VALUES ('{this.GetControl<TextBox>("Emailbus").Text.ToLower()}', '{this.GetControl<TextBox>("Password1bus").Text}', '{this.GetControl<TextBox>("Namebus").Text}', '{this.GetControl<TextBox>("Phonebus").Text.Split("ка ")[1]}');";
                        cmd = new NpgsqlCommand(sql, con);
                        cmd.ExecuteScalar();
                        Errorbus.IsVisible = false;

                        sql = $"SELECT id FROM main_businesses WHERE email = '{this.GetControl<TextBox>("Emailbus").Text.ToLower()}';";
                        cmd = new NpgsqlCommand(sql, con);
                        version = cmd.ExecuteScalar();
                        User.Id = Int64.Parse(version.ToString());
                        User.Model = new BusinessAccountViewModel();
                        User.Main.Page = User.Model;
                    }
                    else
                    {
                        this.GetControl<TextBlock>("Errorbus").IsVisible = true;
                    }
                }
                else
                {
                    this.GetControl<TextBlock>("Errorbus").IsVisible = true;
                }
                con.Close();
            }
        }

        public void Create_Click(object source, RoutedEventArgs args)
        {
            if (this.GetControl<Border>("woodcutter1").IsVisible)
            {
                return;
            }
            else if (this.GetControl<Border>("spaceman1").IsVisible)
            {
                this.GetControl<Border>("spaceman1").IsVisible = false;
                this.GetControl<Border>("spaceman").IsVisible = true;
            }
            this.GetControl<Border>("woodcutter").IsVisible = false;
            this.GetControl<Border>("woodcutter1").IsVisible = true;
        }

        [GeneratedRegex("_")]
        private static partial Regex MyRegex();
    }
}