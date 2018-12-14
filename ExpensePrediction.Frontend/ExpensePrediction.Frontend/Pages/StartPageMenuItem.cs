using System;
using Xamarin.Forms;

namespace ExpensePrediction.Frontend.Pages
{
    public class StartPageMenuItem
    {
        public StartPageMenuItem(int id, string title, Type targetType)
        {
            Id = id;
            Title = title;
            TargetType = targetType;
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}