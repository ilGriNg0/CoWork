using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.ViewModels;
using AvaloniaApplication4.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace AvaloniaApplication4
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = MainWindowViewModel.Instance,
                };
             
            
                
            }

            base.OnFrameworkInitializationCompleted();
        }
       
    }
  
}