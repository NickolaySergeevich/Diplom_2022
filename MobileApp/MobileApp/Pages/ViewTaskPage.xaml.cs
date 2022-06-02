using System;

using MobileApp.ApiJsonResponse;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewTaskPage
    {
        private readonly TasksResponse _tasksResponse;

        public ViewTaskPage(TasksResponse tasksResponse)
        {
            InitializeComponent();

            label_name.Text = tasksResponse.Name;
            label_organization.Text = tasksResponse.Organization;
            label_description.Text = tasksResponse.Description;
            label_teamCount.Text = tasksResponse.TeamsCount != -1 ? tasksResponse.TeamsCount.ToString() : "любое";
            label_teamMemberMax.Text = tasksResponse.TeamMemberMax != -1 ? tasksResponse.TeamMemberMax.ToString() : "любое";
            label_region.Text = tasksResponse.Region ?? "любой";
            label_isEssay.Text = tasksResponse.IsEssay ? "есть" : "нет";
            label_isTest.Text = tasksResponse.IsTest ? "есть" : "нет";

            _tasksResponse = tasksResponse;
        }

        private void Button_signUpToTask_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new SignUpToTaskPage(_tasksResponse);
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainUserPage();
        }
    }
}