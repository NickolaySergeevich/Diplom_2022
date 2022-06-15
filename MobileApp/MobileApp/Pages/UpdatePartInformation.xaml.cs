using System;
using System.Linq;

using MobileApp.ApiJsonRequest;
using MobileApp.ApiJsonResponse;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdatePartInformation
    {
        private readonly RestService _restService;

        private PartInformationResponse _partInformation;
        private readonly string _password;

        public UpdatePartInformation(PartInformationResponse partInformation, string password)
        {
            InitializeComponent();

            _restService = new RestService();

            _partInformation = partInformation;
            _password = password;

            entry_login.Text = _partInformation.Username;
            entry_name.Text = _partInformation.Name;
            entry_surname.Text = _partInformation.Surname;
            entry_patronymic.Text = _partInformation.Patronymic;
            entry_email.Text = _partInformation.Email;
            entry_phone_number.Text = _partInformation.PhoneNumber;
            entry_organization.Text = _partInformation.Organization;
            entry_city.Text = _partInformation.City;
        }

        private async void Button_update_OnClicked(object sender, EventArgs e)
        {
            var fieldsOk = grid_main.Children.Where(child => child.GetType() == typeof(Entry)).All(child => !string.IsNullOrEmpty(((Entry)child).Text));
            if (!fieldsOk)
            {
                await DisplayAlert("Внимание", "Не заполнены поля для обновления", "OK");
                return;
            }

            var request = new UpdatePartRequest
            {
                UserId = _partInformation.UserId,
                Username = entry_login.Text,
                Password = _password,
                Name = entry_name.Text,
                Surname = entry_surname.Text,
                Patronymic = entry_patronymic.Text,
                Email = entry_email.Text,
                PhoneNumber = entry_phone_number.Text,
                Organization = entry_organization.Text,
                City = entry_city.Text
            };

            button_update.IsEnabled = false;

            var response =
                await _restService.GetResponseWithBody<UpdateUserResponse, UpdatePartRequest>(
                    Constants.UpdatePartAddress, request);
            switch (response.Status)
            {
                case Constants.ServerError:
                    await DisplayAlert("Внимание", "Проблемы с сервером. Обратитесь к системному администратору.", "OK");
                    break;
                case "false":
                    await DisplayAlert("Внимание", "Подобные данные уже есть", "OK");
                    break;
                default:
                    _partInformation = await _restService.GetResponseAsync<PartInformationResponse>(Constants.PartInformationAddress + "?user_id=" + _partInformation.UserId + "&username=" + entry_login.Text + "&password=" + _password);

                    await DisplayAlert("Успех", "Ваши данные успешно обновлены!", "OK");
                    break;
            }

            button_update.IsEnabled = true;
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPartPage(_partInformation, _password);
        }
    }
}