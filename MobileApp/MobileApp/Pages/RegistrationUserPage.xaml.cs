using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using MobileApp.ApiJsonRequest;
using MobileApp.ApiJsonResponse;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationUserPage
    {
        private readonly RestService _restService;

        public RegistrationUserPage()
        {
            InitializeComponent();

            _restService = new RestService();
        }

        private async void Button_registration_OnClicked(object sender, EventArgs e)
        {
            var fieldsOk = grid_main.Children.Where(child => child.GetType() == typeof(Entry)).All(child => !string.IsNullOrEmpty(((Entry)child).Text));
            if (!fieldsOk)
            {
                await DisplayAlert("Внимание", "Не заполнены поля для регистрации", "OK");
                return;
            }

            var registrationUserRequest = new RegistrationUserRequest
            {
                Username = entry_login.Text,
                Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(entry_password.Text))).Replace("-", string.Empty),
                Name = entry_name.Text,
                Surname = entry_surname.Text,
                Patronymic = entry_patronymic.Text,
                Country = entry_country.Text,
                City = entry_city.Text,
                EducationalInstitution = entry_educationalInstitution.Text,
                ClassNumber = Convert.ToInt32(entry_classNumber.Text),
                Email = entry_email.Text,
                PhoneNumber = entry_phoneNumber.Text
            };

            button_registration.IsEnabled = false;

            var response =
                await _restService.GetResponseWithBody<RegistrationResponse, RegistrationUserRequest>(Constants.RegistrationUserAddress, registrationUserRequest);
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
                    entry_patronymic.Text = string.Empty;
                    entry_country.Text = string.Empty;
                    entry_city.Text = string.Empty;
                    entry_educationalInstitution.Text = string.Empty;
                    entry_classNumber.Text = string.Empty;
                    entry_email.Text = string.Empty;
                    entry_phoneNumber.Text = string.Empty;

                    await DisplayAlert("Успех", "Вы успешно зарегистрированы!", "OK");
                    break;
            }

            button_registration.IsEnabled = true;
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}