using System;


namespace AvaloniaApplication4.Models;

public class CardModel 
{
        public string Label { get; }
        private Type _mode_tp;

        public CardModel(Type tp)
        {
            _mode_tp = tp;
            Label = tp.Name.Replace("ViewModel", "");
        }
}