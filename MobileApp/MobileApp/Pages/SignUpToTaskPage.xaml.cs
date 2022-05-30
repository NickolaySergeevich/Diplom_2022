using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using MobileApp.ApiJsonRequest;
using MobileApp.ApiJsonResponse;
using MobileApp.ViewCells;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpToTaskPage
    {
        private readonly RestService _restService;

        private readonly TasksResponse _tasksResponse;
        private readonly LoginResponse _loginResponse;

        private ObservableCollection<TeamMemberCellClass> _members;
        public ObservableCollection<TeamMemberCellClass> Members => _members ?? (_members = new ObservableCollection<TeamMemberCellClass>());

        public SignUpToTaskPage(TasksResponse tasksResponse, LoginResponse loginResponse)
        {
            InitializeComponent();

            _restService = new RestService();

            _tasksResponse = tasksResponse;
            _loginResponse = loginResponse;

            label_taskName.Text = _tasksResponse.Name;
            label_taskOrganization.Text = _tasksResponse.Organization;

            for (var i = 0; i < _tasksResponse.TeamMemberMax; ++i)
                Members.Add(new TeamMemberCellClass { Username = string.Empty, IsTeamLead = false });

            listView_group.ItemsSource = Members;
        }

        private async void Button_signUp_OnClicked(object sender, EventArgs e)
        {
            var memberTeamLead = from member in _members where member.IsTeamLead select member;
            var teamLead = memberTeamLead as TeamMemberCellClass[] ?? memberTeamLead.ToArray();
            if (teamLead.Length > 1)
            {
                await DisplayAlert("Внимание", "Не может быть больше одного тимлидера!", "OK");
                return;
            }
            if (!teamLead.Any())
            {
                await DisplayAlert("Внимание", "Должен быть хотя бы один тимлидер!", "OK");
                return;
            }
            if (string.IsNullOrEmpty(teamLead.First().Username))
            {
                await DisplayAlert("Внимание", "У тимлидера должен быть указан логин!", "OK");
                return;
            }

            button_signUp.IsEnabled = false;

            var usersId = new List<int>();
            var error = false;
            foreach (var member in (from member in _members where !string.IsNullOrEmpty(member.Username) select member))
            {
                var response =
                    await _restService.GetUserIdResponseAsync(Constants.GetUserIdAddress + "?username=" +
                                                              member.Username);
                switch (response.Status)
                {
                    case Constants.ServerError:
                        await DisplayAlert("Ooops", "С сервером что-то не так. Обратитесь к системному администратору.", "OK");
                        error = true;
                        break;
                    case Constants.NoDataInDb:
                        await DisplayAlert("Вы не зарегистрированы!", "Не можем найти ваши данные на сервере. Пользователь не зарегистрирован.", "OK");
                        error = true;
                        break;
                    default:
                        usersId.Add(response.Id);
                        break;
                }

                if (error)
                    break;
            }

            if (!error)
            {
                var signUpToTask = new SignUpToTaskRequest
                {
                    UsersId = usersId,
                    TaskId = _tasksResponse.Id
                };
                var response =
                    await _restService.GetSignUpToTaskResponseAsync(Constants.SignUpToTaskAddress, signUpToTask);
                switch (response.Status)
                {
                    case Constants.ServerError:
                        await DisplayAlert("Внимание", "Проблемы с сервером. Обратитесь к системному администратору.", "OK");
                        break;
                    case "false":
                        await DisplayAlert("Внимание", "Один из указанных пользователей уже зарегистрирован на конкурс.", "OK");
                        break;
                    default:
                        foreach (var member in _members)
                        {
                            member.Username = string.Empty;
                            member.IsTeamLead = false;
                        }

                        await DisplayAlert("Успех", "Вы успешно зарегистрированы!", "OK");
                        break;
                }
            }

            button_signUp.IsEnabled = true;
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new ViewTaskPage(_tasksResponse, _loginResponse);
        }
    }
}