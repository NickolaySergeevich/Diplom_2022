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
        private readonly LoginResponse _loginResponse;

        public ViewTaskPage(TasksResponse tasksResponse, LoginResponse loginResponse)
        {
            InitializeComponent();

            _tasksResponse = tasksResponse;
            _loginResponse = loginResponse;

            label_name.Text = _tasksResponse.Name;
            label_organization.Text = _tasksResponse.Organization;
            label_description.Text = "Описание:\n" + _tasksResponse.Description;

            label_region.Text = "Регион: ";
            if (_tasksResponse.Region == null)
                label_region.Text += "любой";
            else
                label_region.Text += _tasksResponse.Region;

            label_teamsCount.Text = "Команды: ";
            if (_tasksResponse.TeamsCount == -1)
                label_teamsCount.Text += "не ограничено";
            else
                label_teamsCount.Text += _tasksResponse.TeamsCount;

            label_isEssay.Text = "Эссе: ";
            if (_tasksResponse.IsEssay)
                label_isEssay.Text += "есть";
            else
                label_isEssay.Text += "нет";

            label_isTest.Text = "Тест: ";
            if (_tasksResponse.IsTest)
                label_isTest.Text += "есть";
            else
                label_isTest.Text += "нет";
            _loginResponse = loginResponse;
        }

        private void Button_insert_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new SignUpToTaskPage();
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainUserPage(_loginResponse);
        }
    }
}