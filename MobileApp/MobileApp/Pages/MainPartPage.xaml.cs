using System;

using MobileApp.ApiJsonResponse;

using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPartPage
    {
        private readonly RestService _restService;

        private readonly PartInformationResponse _partInformation;
        private readonly string _password;

        public MainPartPage(PartInformationResponse partInformation, string password)
        {
            InitializeComponent();

            _restService = new RestService();

            _partInformation = partInformation;
            _password = password;

            label_login.Text = _partInformation.Username;
            label_name.Text = _partInformation.Name;
            label_surname.Text = _partInformation.Surname;
            label_patronymic.Text = _partInformation.Patronymic;
            label_email.Text = _partInformation.Email;
            label_phoneNumber.Text = _partInformation.PhoneNumber;
            label_organization.Text = _partInformation.Organization;
            label_city.Text = _partInformation.City;
        }

        private void Button_changeInformation_OnClicked(object sender, EventArgs e)
        {

        }

        private void Button_downloadTeams_OnClicked(object sender, EventArgs e)
        {

        }
    }
}