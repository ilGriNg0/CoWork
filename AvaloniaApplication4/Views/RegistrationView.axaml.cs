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
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
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
            User.Main.CurrentPage = User.Model;
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
            if (spaceman1.IsVisible)
            {
                return;
            }
            else if (woodcutter1.IsVisible)
            {
                woodcutter1.IsVisible = false;
                woodcutter.IsVisible = true;
            }
            spaceman1.IsVisible = true;
            spaceman.IsVisible = false;
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
                    if (dateper && (MyRegex().Matches(textBox.Text).Count != 0 || !CheckDate(textBox.Text.Split("�� ")[1]))) { textBox.BorderBrush = newbrush; person = SetBit(person, 4, 1); }
                        else { textBox.BorderBrush = lastbrush; dateper = true; person = SetBit(person, 4, 0); }
                    break;

                case "Password1per":
                    if (textBox.Text == null || textBox.Text.Length < 8) { textBox.BorderBrush = newbrush; person = SetBit(person, 5, 1); }
                        else { textBox.BorderBrush = lastbrush; person = SetBit(person, 5, 0); }
                    if (Password2per.Text != null && !textBox.Text.Equals(Password2per.Text)) { Password2per.BorderBrush = newbrush; person = SetBit(person, 6, 1); }
                        else { Password2per.BorderBrush = lastbrush; person = SetBit(person, 6, 0); }
                    break;

                case "Password2per":
                    if (!textBox.Text.Equals(Password1per.Text)) { textBox.BorderBrush = newbrush; person = SetBit(person, 6, 1); }
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
                    if (Password2bus.Text != null && !textBox.Text.Equals(Password2bus.Text)) { Password2bus.BorderBrush = newbrush; business = SetBit(business, 4, 1); }
                        else { Password2bus.BorderBrush = lastbrush; business = SetBit(business, 4, 0); }
                    break;

                case "Password2bus":
                    if (!textBox.Text.Equals(Password1bus.Text)) { textBox.BorderBrush = newbrush; business = SetBit(business, 4, 1); }
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
                var sql = $"SELECT count(*) FROM main_users WHERE email = '{Emailper.Text.ToLower()}';";
                var cmd = new NpgsqlCommand(sql, con);
                var version = cmd.ExecuteScalar();

                if (version.ToString() == "0")
                {
                    sql = $"SELECT count(*) FROM main_businesses WHERE email = '{Emailper.Text.ToLower()}';";
                    cmd = new NpgsqlCommand(sql, con);
                    version = cmd.ExecuteScalar();

                    if (version.ToString() == "0")
                    {
                        sql = $"INSERT INTO main_users(email, password, first_name, last_name, phone_number, date_of_birth) " +
                            $"VALUES ('{Emailper.Text.ToLower()}', '{Password1per.Text}', '{Firstper.Text}', '{Lastper.Text}', " +
                            $"'{Phoneper.Text.Split("�� ")[1]}', '{DateTime.Parse(Dateper.Text.Split("�� ")[1]).ToShortDateString()}');";
                        cmd = new NpgsqlCommand(sql, con);
                        cmd.ExecuteScalar();
                        Errorper.IsVisible = false;

                        sql = $"SELECT id FROM main_users WHERE email = '{Emailper.Text.ToLower()}';";
                        cmd = new NpgsqlCommand(sql, con);
                        version = cmd.ExecuteScalar();
                        User.Id = Int64.Parse(version.ToString());
                        User.Type = "users";
                        User.Model = new PersonalAccountViewModel();
                        User.Main.CurrentPage = User.Model;

                        var box = MessageBoxManager.GetMessageBoxStandard("", "�������� ����������� ������������� ��������", ButtonEnum.Ok);
                        box.ShowAsync();
                    }
                    else
                    {
                        Errorper.IsVisible = true;
                    }
                }
                else
                {
                    Errorper.IsVisible = true;
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
                var sql = $"SELECT count(*) FROM main_businesses WHERE email = '{Emailbus.Text.ToLower()}';";
                var cmd = new NpgsqlCommand(sql, con);
                var version = cmd.ExecuteScalar();

                if (version.ToString() == "0")
                {
                    sql = $"SELECT count(*) FROM main_users WHERE email = '{Emailbus.Text.ToLower()}';";
                    cmd = new NpgsqlCommand(sql, con);
                    version = cmd.ExecuteScalar();

                    if (version.ToString() == "0")
                    {
                        sql = $"INSERT INTO main_businesses(email, password, company_name, phone_number) " +
                            $"VALUES ('{Emailbus.Text.ToLower()}', '{Password1bus.Text}', '{Namebus.Text}', '{Phonebus.Text.Split("�� ")[1]}');";
                        cmd = new NpgsqlCommand(sql, con);
                        cmd.ExecuteScalar();
                        Errorbus.IsVisible = false;

                        sql = $"SELECT id FROM main_businesses WHERE email = '{Emailbus.Text.ToLower()}';";
                        cmd = new NpgsqlCommand(sql, con);
                        version = cmd.ExecuteScalar();
                        User.Id = Int64.Parse(version.ToString());
                        User.Type = "businesses";
                        User.Model = new BusinessAccountViewModel();
                        User.Main.CurrentPage = User.Model;

                        var box = MessageBoxManager.GetMessageBoxStandard("", "�������� ����������� ������-��������", ButtonEnum.Ok);
                        box.ShowAsync();
                    }
                    else
                    {
                        Errorbus.IsVisible = true;
                    }
                }
                else
                {
                    Errorbus.IsVisible = true;
                }
                con.Close();
            }
        }

        public void Create_Click(object source, RoutedEventArgs args)
        {
            if (woodcutter1.IsVisible)
            {
                return;
            }
            else if (spaceman1.IsVisible)
            {
                spaceman1.IsVisible = false;
                spaceman.IsVisible = true;
            }
            woodcutter.IsVisible = false;
            woodcutter1.IsVisible = true;
        }

        [GeneratedRegex("_")]
        private static partial Regex MyRegex();
    }
}