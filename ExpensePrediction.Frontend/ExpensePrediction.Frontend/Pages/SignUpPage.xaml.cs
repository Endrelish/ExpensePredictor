﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects.User;
using ExpensePrediction.Exceptions;
using ExpensePrediction.Frontend.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensePrediction.Frontend.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private readonly AuthService _authService;
        public SignUpPage()
        {
            _authService = new AuthService();
            InitializeComponent();
        }

        private async void SignUpClicked(object sender, EventArgs e)
        {
            await ActivityIndicatorPage.ToggleIndicator(true);
            if (Password != PasswordConfirm)
            {
                await DisplayAlert("", "Passwords don't match", "");
            }

            var registerDto = new RegisterDto
            {
                Email = Email.Text,
                Username = Username.Text,
                Password = Password.Text,
                FirstName = FirstName.Text,
                LastName = LastName.Text,
                PhoneNumber = Phone.Text
            };

            try
            {
                void Navigate()
                {
                    Application.Current.MainPage = new StartPage();
                }

                await _authService.RegisterAsync(registerDto, Navigate);
            }
            catch (RestException re)
            {
                await DisplayAlert("", re.Message, "");
            }
            finally
            {
                await ActivityIndicatorPage.ToggleIndicator(false);
            }
        }
    }
}