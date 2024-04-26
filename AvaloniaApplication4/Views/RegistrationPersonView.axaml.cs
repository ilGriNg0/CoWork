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

namespace AvaloniaApplication4.Views
{
    public partial class RegistrationPersonView : UserControl
    {
        public RegistrationPersonView()
        {
            InitializeComponent();
        }

        public void Find_Click(object source, RoutedEventArgs args)
        {   
            if (this.GetControl<Border>("spaceman1").IsVisible)
            {
                Debug.WriteLine("ok");
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
        public void Create_Click(object source, RoutedEventArgs args)
        {
            if (this.GetControl<Border>("woodcutter1").IsVisible)
            {
                Debug.WriteLine("not ok");
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
    }
}