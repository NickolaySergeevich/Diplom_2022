using System;
using System.Linq;

using MobileApp.ApiJsonRequest;
using MobileApp.ApiJsonResponse;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateOrgInformationPage
    {
        private readonly RestService _restService;

        private OrgInformationResponse _orgInformation;
        private readonly string _password;

        public UpdateOrgInformationPage(OrgInformationResponse orgInformation, string password)
        {
            InitializeComponent();

            _restService = new RestService();

            _orgInformation = orgInformation;
            _password = password;

            entry_login.Text = _orgInformation.Username;
            entry_name.Text = _orgInformation.Name;
            entry_surname.Text = _orgInformation.Surname;
            entry_patronymic.Text = _orgInformation.Patronymic;
            entry_email.Text = _orgInformation.Email;
        }

        private async void Button_update_OnClicked(object sender, EventArgs e)
        {
            var fieldsOk = grid_main.Children.Where(child => child.GetType() == typeof(Entry)).All(child => !string.IsNullOrEmpty(((Entry)child).Text));
            if (!fieldsOk)
            {
                await DisplayAlert("Внимание", "Не заполнены поля для обновления", "OK");
                return;
            }

            var request = new UpdateOrgRequest
            {
                UserId = _orgInformation.UserId,
                Username = entry_login.Text,
                Password = _password,
                Name = entry_name.Text,
                Surname = entry_surname.Text,
                Patronymic = entry_patronymic.Text,
                Email = entry_email.Text
            };

            button_update.IsEnabled = false;

            var response =
                await _restService.GetResponseWithBody<UpdateUserResponse, UpdateOrgRequest>(
                    Constants.UpdateOrgAddress, request);
            switch (response.Status)
            {
                case Constants.ServerError:
                    await DisplayAlert("Внимание", "Проблемы с сервером. Обратитесь к системному администратору.", "OK");
                    break;
                case "false":
                    await DisplayAlert("Внимание", "Подобные данные уже есть", "OK");
                    break;
                default:
                    _orgInformation = await _restService.GetResponseAsync<OrgInformationResponse>(Constants.OrgInformationAddress + "?user_id=" + _orgInformation.UserId + "&username=" + entry_login.Text + "&password=" + _password);

                    await DisplayAlert("Успех", "Ваши данные успешно обновлены!", "OK");
                    break;
            }

            button_update.IsEnabled = true;
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainOrgPage(_orgInformation, _password);
        }
    }
}