using System;
using System.IO;

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

        [Obsolete("Obsolete")]
        private async void Button_downloadTeams_OnClicked(object sender, EventArgs e)
        {
            button_downloadTeams.IsEnabled = false;

            var response = await _restService.GetResponseAsync<GetTeamsResponse>(Constants.GetTeamsByPartAddress +
                "?username=" + _partInformation.Username + "&password=" + _password + "&organization_name=" + _partInformation.Organization);
            switch (response.Status)
            {
                case Constants.ServerError:
                    await DisplayAlert("Внимание", "Неполадки с сервером! Обратитесь к системному администратору!",
                        "Ok");
                    break;
                case Constants.NoDataInDb:
                    await DisplayAlert("Внимание", "У вас неправильный id! Обратитесь к системному администратору!",
                        "Ok");
                    break;
                default:
                    var answer = string.Empty;
                    foreach (var dataKey in response.Data.Keys)
                    {
                        answer += "Название конкурса: " + dataKey + "\n";

                        foreach (var key in response.Data[dataKey].Keys)
                        {
                            answer += "\tНазвание команды: " + key + "\n";

                            foreach (var teamsListType in response.Data[dataKey][key])
                            {
                                answer += "\t\tИмя: " + teamsListType.Name + "\n";
                                answer += "\t\tФамилия: " + teamsListType.Surname + "\n";
                                answer += "\t\tОтчество: " + teamsListType.Patronymic + "\n";
                                answer += "\t\tEmail: " + teamsListType.Email + "\n";
                                if (teamsListType.IsTeamLead)
                                    answer += "\t\tТИМЛИДЕР\n";

                                answer += "\n";
                            }
                        }
                    }

                    var absolutePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments)?.AbsolutePath;
                    if (absolutePath != null)
                    {
                        File.WriteAllText(Path.Combine(absolutePath, $"teams_{_partInformation.Organization}.txt"), answer);
                        await DisplayAlert("Успех!", "Файл с командами сохранен в документы!", "ok");
                    }

                    break;
            }

            button_downloadTeams.IsEnabled = true;
        }
    }
}