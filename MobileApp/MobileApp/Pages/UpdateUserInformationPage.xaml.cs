using System;
using System.Linq;

using MobileApp.ApiJsonRequest;
using MobileApp.ApiJsonResponse;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateUserInformationPage
    {
        private readonly RestService _restService;

        private UserInformationResponse _userInformation;
        private readonly string _password;

        public UpdateUserInformationPage(UserInformationResponse userInformation, string password)
        {
            InitializeComponent();

            _restService = new RestService();

            _userInformation = userInformation;
            _password = password;

            entry_login.Text = _userInformation.Username;
            entry_name.Text = _userInformation.Name;
            entry_surname.Text = _userInformation.Surname;
            entry_patronymic.Text = _userInformation.Patronymic;
            entry_country.Text = _userInformation.Country;
            entry_city.Text = _userInformation.City;
            entry_educationalInstitution.Text = _userInformation.EducationalInstitution;
            entry_classNumber.Text = _userInformation.ClassNumber.ToString();
            entry_email.Text = _userInformation.Email;
            entry_phoneNumber.Text = _userInformation.PhoneNumber;
        }

        private async void Button_update_OnClicked(object sender, EventArgs e)
        {
            var fieldsOk = grid_main.Children.Where(child => child.GetType() == typeof(Entry)).All(child => !string.IsNullOrEmpty(((Entry)child).Text));
            if (!fieldsOk)
            {
                await DisplayAlert("Внимание", "Не заполнены поля для обновления", "OK");
                return;
            }

            var request = new UpdateUserRequest
            {
                UserId = _userInformation.UserId,
                Username = entry_login.Text,
                Password = _password,
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

            button_update.IsEnabled = false;

            var response =
                await _restService.GetResponseWithBody<UpdateUserResponse, UpdateUserRequest>(
                    Constants.UpdateUserAddress, request);
            switch (response.Status)
            {
                case Constants.ServerError:
                    await DisplayAlert("Внимание", "Проблемы с сервером. Обратитесь к системному администратору.", "OK");
                    break;
                case "false":
                    await DisplayAlert("Внимание", "Подобные данные уже есть", "OK");
                    break;
                default:
                    _userInformation = await _restService.GetResponseAsync<UserInformationResponse>(Constants.UserInformationAddress + "?user_id=" + _userInformation.UserId + "&username=" + entry_login.Text + "&password=" + _password);

                    await DisplayAlert("Успех", "Ваши данные успешно обновлены!", "OK");
                    break;
            }

            button_update.IsEnabled = true;
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainUserPage(_userInformation, _password);
        }
    }
}