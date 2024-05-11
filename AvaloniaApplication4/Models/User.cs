using AvaloniaApplication4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication4.Models
{
    public class User
    {
        public static long Id { get; set;}
        public static string Type { get; set;}
        public static string Phone { get; set;}
        public static string Email { get; set;}
        public static string Password { get; set;}
        public static ViewModelBase Model { get; set;}
        public static MainWindowViewModel Main {  get; set;}
    }
}
