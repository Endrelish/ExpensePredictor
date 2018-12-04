using System;

namespace ExpensePrediction.Frontend.Pages
{
    public class StartPageMenuItem
    {
        public StartPageMenuItem()
        {
            TargetType = typeof(StartPageDetail);
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}