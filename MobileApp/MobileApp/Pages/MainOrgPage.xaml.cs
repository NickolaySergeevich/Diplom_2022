using System;
using System.IO;

using MobileApp.ApiJsonResponse;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainOrgPage
    {
        private readonly RestService _restService;

        private readonly OrgInformationResponse _orgInformation;
        private readonly string _password;

        public MainOrgPage(OrgInformationResponse orgInformation, string password)
        {
            InitializeComponent();

            _restService = new RestService();

            _orgInformation = orgInformation;
            _password = password;

            label_login.Text = _orgInformation.Username;
            label_name.Text = _orgInformation.Name;
            label_surname.Text = _orgInformation.Surname;
            label_patronymic.Text = _orgInformation.Patronymic;
            label_email.Text = _orgInformation.Email;
        }

        private void Button_changeInformation_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new UpdateOrgInformationPage(_orgInformation, _password);
        }

        [Obsolete("Obsolete")]
        private async void Button_downloadTeams_OnClicked(object sender, EventArgs e)
        {
            button_downloadTeams.IsEnabled = false;

            var response = await _restService.GetResponseAsync<GetTeamsResponse>(Constants.GetTeamsAddress +
                "?username=" + _orgInformation.Username + "&password=" + _password);
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
                        File.WriteAllText(Path.Combine(absolutePath, "teams.txt"), answer);
                        await DisplayAlert("Успех!", "Файл с командами сохранен в документы!", "ok");
                    }

                    break;
            }

            button_downloadTeams.IsEnabled = true;
        }

        private void Button_loadWorks_OnClicked(object sender, EventArgs e)
        {
            button_loadWorks.Text = "В разработке";
        }
    }
}
