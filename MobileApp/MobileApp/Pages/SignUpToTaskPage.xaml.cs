using System;
using System.Collections.ObjectModel;

using MobileApp.ApiJsonResponse;
using MobileApp.ViewCells;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpToTaskPage : ContentPage
    {
        private readonly TasksResponse _tasksResponse;
        private readonly LoginResponse _loginResponse;

        private ObservableCollection<TeamMemberCell> _members;
        public ObservableCollection<TeamMemberCell> Members => _members ?? (_members = new ObservableCollection<TeamMemberCell>());

        public SignUpToTaskPage(TasksResponse tasksResponse, LoginResponse loginResponse)
        {
            InitializeComponent();

            _tasksResponse = tasksResponse;
            _loginResponse = loginResponse;

            label_taskName.Text = _tasksResponse.Name;
            label_taskOrganization.Text = _tasksResponse.Organization;

            for (var i = 0; i < _tasksResponse.TeamMemberMax; ++i)
                Members.Add(new TeamMemberCell());

            listView_group.ItemsSource = Members;
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new ViewTaskPage(_tasksResponse, _loginResponse);
        }
    }
}