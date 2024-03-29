﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensePrediction.Frontend.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : MasterDetailPage
    {
        public StartPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as StartPageMenuItem;
            if (item == null)
            {
                return;
            }
            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;
            Detail = new NavigationPage(page);
            IsPresented = false;
            MasterPage.ListView.SelectedItem = null;

            if (page is IInitializedPage p) await p.Initialize();
        }
    }
}