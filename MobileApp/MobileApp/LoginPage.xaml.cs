﻿using System;
using System.Security.Cryptography;
using System.Text;

using Xamarin.Forms;

namespace MobileApp
{
    public partial class MainPage
    {
        private readonly RestService _restService;

        public MainPage()
        {
            InitializeComponent();

            _restService = new RestService();
        }

        private async void Button_login_Clicked(object sender, EventArgs e)
        {
            if (entry_login.Text == string.Empty || entry_login.Text == string.Empty)
            {
                await DisplayAlert("Внимание", "Не заполнены поля для входа", "OK");
                return;
            }

            var login = entry_login.Text;
            var password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(entry_password.Text))).Replace("-", string.Empty);

            var response = await _restService.GetLoginResponseAsync(Constants.LoginAddress + "?username=" + login + "&password=" + password);
            switch (response.Status)
            {
                case Constants.ServerError:
                    await DisplayAlert("Ooops", "С сервером что-то не так. Обратитесь к системному администратору.", "OK");
                    return;
                case Constants.NoDataInDb:
                    await DisplayAlert("Вы не зарегистрированы!", "Не можем найти ваши данные на сервере. Пройдите регистрацию.", "OK");
                    return;
                default:
                    Application.Current.MainPage = new MainUserPage();
                    break;
            }

        }

        private void Button_registration_Clicked(object sender, EventArgs e)
        {

        }
    }
}
