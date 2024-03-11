//using System;
//using System.Reflection;
//using Avalonia.Media.Imaging;
//using Avalonia.Media.TextFormatting;
//using Avalonia.Platform;
//using Microsoft.CodeAnalysis.Scripting;

//namespace AvaloniaApplication4;

//public static class ImageLoad
//{
//    public static Bitmap Load(string path)
//    {
//        Uri res_uri;
//        if (!path.StartsWith("avares://"))
//        {
//            var assemb_name = Assembly.GetEntryAssembly()?.GetName().Name;
//            res_uri = new Uri($"avares://{assemb_name}/{path.TrimStart('/')}");
//        }
//        else
//        {
//            res_uri = new Uri(path);
//        }

//        return new Bitmap(AssetLoader.Open(res_uri));
//    }

//}