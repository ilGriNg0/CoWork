using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using AvaloniaApplication4.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace AvaloniaApplication4.Views;

public partial class AddCardView : UserControl
{
    public AddCardView()
    {
        InitializeComponent();
        //link(Text);
    }
  static public ObservableCollection<JsonClass> strings { get; set; } = new();
    JsonClass js = new();
    private void OnAddTextBoxButtonClick(object? sender, RoutedEventArgs e)
    {
        AddTextBox();

    }

   
    private void OnRemoveTextBoxButtonClick(object? sender, RoutedEventArgs e)
    {
        //RemoveLastTextBox();
    }
    public string Text { get; set; }
    private void AddTextBox()
    {
        var stackPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            Margin = new Avalonia.Thickness(0, 5, 0, 0)
        };

        var textBox = new TextBox
        {
            Width = 550,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,

            Margin = new Avalonia.Thickness(5, 5, 5, 5)
        };


        textBox.Bind(TextBox.TextProperty, new Binding("Text") { Source = this, Mode = BindingMode.TwoWay });
        link(Text);


        var deleteButton = new Button
        {
            Content = "Удалить",
            Background = Brushes.Gray,
            Tag = stackPanel // Используем Tag для хранения ссылки на StackPanel
        };
        deleteButton.Click += OnDeleteButtonClick;

        stackPanel.Children.Add(textBox);
        stackPanel.Children.Add(deleteButton);

        //TextBoxContainer.Children.Add(stackPanel);
    }
    public void link(string txt)
{
        if (txt != null)
        {
            var its = new JsonClass { Info_cowork = Text };
            strings.Add(its);
         
        }
    }

    //private void RemoveLastTextBox()
    //{
    //    if (TextBoxContainer.Children.Count > 0)
    //    {
    //        var lastChild = TextBoxContainer.Children[TextBoxContainer.Children.Count - 1];
    //        if (lastChild is StackPanel stackPanel && stackPanel.Children.Count > 0)
    //        {
    //            // Отписываемся от события, чтобы избежать утечек памяти
    //            var deleteButton = stackPanel.Children[1] as Button;
    //            if (deleteButton != null)
    //            {
    //                deleteButton.Click -= OnDeleteButtonClick;
    //            }
    //            TextBoxContainer.Children.Remove(lastChild);
    //        }
    //    }
    //}

    private void OnDeleteButtonClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is StackPanel stackPanel)
        {
            //TextBoxContainer.Children.Remove(stackPanel);
        }
    }
}