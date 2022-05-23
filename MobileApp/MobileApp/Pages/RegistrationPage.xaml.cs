using System;
using System.Security.Cryptography;
using System.Text;

using MobileApp.ApiJsonRequest;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage
    {
        private readonly RestService _restService;

        public RegistrationPage()
        {
            InitializeComponent();

            _restService = new RestService();
        }

        private async void Button_registration_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entry_login.Text) || string.IsNullOrEmpty(entry_password.Text) || string.IsNullOrEmpty(entry_name.Text) || string.IsNullOrEmpty(entry_surname.Text))
            {
                await DisplayAlert("Внимание", "Не заполнены поля для регистрации", "OK");
                return;
            }

            var registrationRequest = new RegistrationRequest
            {
                Username = entry_login.Text,
                Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(entry_password.Text))).Replace("-", string.Empty),
                Name = entry_name.Text,
                Surname = entry_surname.Text
            };

            var response =
                await _restService.GetRegistrationResponseAsync(Constants.RegistrationAddress, registrationRequest);
            switch (response.Status)
            {
                case Constants.ServerError:
                    await DisplayAlert("Внимание", "Проблемы с сервером. Обратитесь к системному администратору.", "OK");
                    break;
                case "false":
                    await DisplayAlert("Внимание", "Такой пользователь уже есть", "OK");
                    break;
                default:
                    entry_login.Text = string.Empty;
                    entry_password.Text = string.Empty;
                    entry_name.Text = string.Empty;
                    entry_surname.Text = string.Empty;

                    await DisplayAlert("Успех", "Вы успешно зарегистрированы!", "OK");
                    break;
            }
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}