using System;

using MobileApp.ApiJsonResponse;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewTaskPage
    {
        private readonly UserInformationResponse _userInformation;
        private readonly TasksResponse _tasksResponse;
        private readonly string _password;

        public ViewTaskPage(UserInformationResponse userInformation, TasksResponse tasksResponse, string password)
        {
            InitializeComponent();

            _userInformation = userInformation;
            _tasksResponse = tasksResponse;
            _password = password;

            label_name.Text = tasksResponse.Name;
            label_organization.Text = tasksResponse.Organization;
            label_description.Text = tasksResponse.Description;
            label_teamCount.Text = tasksResponse.TeamsCount != -1 ? tasksResponse.TeamsCount.ToString() : "любое";
            label_teamMemberMax.Text = tasksResponse.TeamMemberMax != -1 ? tasksResponse.TeamMemberMax.ToString() : "любое";
            label_region.Text = tasksResponse.Region ?? "любой";
            label_isEssay.Text = tasksResponse.IsEssay ? "есть" : "нет";
            label_isTest.Text = tasksResponse.IsTest ? "есть" : "нет";
        }

        private void Button_signUpToTask_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new SignUpToTaskPage(_userInformation, _tasksResponse, _password);
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainUserPage(_userInformation, _password);
        }
    }
}