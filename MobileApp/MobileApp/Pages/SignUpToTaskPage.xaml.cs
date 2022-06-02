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

        private ObservableCollection<TeamMemberCellClass> _members;
        public ObservableCollection<TeamMemberCellClass> Members => _members ?? (_members = new ObservableCollection<TeamMemberCellClass>());

        public SignUpToTaskPage(TasksResponse tasksResponse)
        {
            InitializeComponent();

            _restService = new RestService();

            _tasksResponse = tasksResponse;

            label_taskName.Text = _tasksResponse.Name;
            label_taskOrganization.Text = _tasksResponse.Organization;

            int teamMemberCounter = 3;
            if (_tasksResponse.TeamMemberMax != -1 && _tasksResponse.TeamMemberMax > teamMemberCounter)
                teamMemberCounter = _tasksResponse.TeamMemberMax;

            for (var i = 0; i < teamMemberCounter; ++i)
                Members.Add(new TeamMemberCellClass { Username = string.Empty, IsTeamLead = false });

            listView_teamMember.ItemsSource = Members;
        }

        private async void Button_signUp_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entry_commandName.Text))
            {
                await DisplayAlert("Внимание", "У команды должно быть название!", "OK");
                return;
            }

            if (string.IsNullOrEmpty(entry_nastUsername.Text))
            {
                await DisplayAlert("Внимание", "Нужно указать логин наставника!", "OK");
                return;
            }

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

            var memberList = from member in _members where !string.IsNullOrEmpty(member.Username) select member;
            var members = memberList as TeamMemberCellClass[] ?? memberList.ToArray();
            if (members.Length < 3)
            {
                await DisplayAlert("Внимание", "Минимум три участника!", "OK");
                return;
            }

            button_signUp.IsEnabled = false;

            var error = false;

            var nastId = -1;
            var responseNastId =
                await _restService.GetResponseAsync<UserIdResponse>(Constants.GetNastIdAddress + "?username=" +
                                                                    entry_nastUsername.Text);
            switch (responseNastId.Status)
            {
                case Constants.ServerError:
                    await DisplayAlert("Ooops", "С сервером что-то не так. Обратитесь к системному администратору.", "OK");
                    error = true;
                    break;
                case Constants.NoDataInDb:
                    await DisplayAlert("Вы не зарегистрированы!", "Не можем найти данные наставника на сервере. Пользователь не зарегистрирован.", "OK");
                    error = true;
                    break;
                default:
                    nastId = responseNastId.Id;
                    break;
            }

            var isTeamLeadId = -1;
            var usersId = new List<int>();
            foreach (var member in members)
            {
                var response =
                    await _restService.GetResponseAsync<UserIdResponse>(Constants.GetUserIdAddress + "?username=" + member.Username);
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

                if (member.IsTeamLead)
                    isTeamLeadId = response.Id;
            }

            if (!error)
            {
                var signUpToTask = new SignUpToTaskRequest
                {
                    UsersId = usersId,
                    TaskId = _tasksResponse.Id,
                    NastId = nastId,
                    CommandName = entry_commandName.Text,
                    IsTeamLeadId = isTeamLeadId
                };

                var response =
                    await _restService.GetResponseWithBody<SignUpToTaskResponse, SignUpToTaskRequest>(Constants.SignUpToTaskAddress, signUpToTask);
                switch (response.Status)
                {
                    case Constants.ServerError:
                        await DisplayAlert("Внимание", "Проблемы с сервером. Обратитесь к системному администратору.", "OK");
                        break;
                    case "false":
                        await DisplayAlert("Внимание", "Один из указанных пользователей уже зарегистрирован на конкурс.", "OK");
                        break;
                    default:
                        await DisplayAlert("Успех", "Вы успешно зарегистрированы!", "OK");

                        Application.Current.MainPage = new ViewTaskPage(_tasksResponse);

                        return;
                }
            }

            button_signUp.IsEnabled = true;
        }

        private void Button_exit_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new ViewTaskPage(_tasksResponse);
        }
    }
}