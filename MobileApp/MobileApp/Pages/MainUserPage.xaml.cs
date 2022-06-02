using System;
using System.Collections.ObjectModel;

using MobileApp.ApiJsonResponse;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainUserPage
    {
        private readonly RestService _restService;
        private readonly LoginResponse _loginResponse;

        private ObservableCollection<TasksResponse> _tasks;
        public ObservableCollection<TasksResponse> Tasks => _tasks ?? (_tasks = new ObservableCollection<TasksResponse>());

        private ObservableCollection<TasksResponse> _tasksByUser;
        public ObservableCollection<TasksResponse> TasksByUser => _tasksByUser ?? (_tasksByUser = new ObservableCollection<TasksResponse>());

        public MainUserPage(LoginResponse loginResponse)
        {
            InitializeComponent();

            _restService = new RestService();

            /*_loginResponse = loginResponse;
            label_name.Text = "Имя: " + _loginResponse.Name;
            label_surname.Text = "Фамилия: " + _loginResponse.Surname;
            label_login.Text = "Логин: " + _loginResponse.Username;*/

            listView_tasks.ItemsSource = Tasks;
            listView_groups.ItemsSource = TasksByUser;
        }

        protected override void OnAppearing()
        {
            UpdateTasks();
            UpdateUserTasks();
        }

        private async void UpdateTasks()
        {
            /*var tasks = await _restService.GetTasksResponseAsync(Constants.TasksAddress);
            foreach (var task in tasks)
                Tasks.Add(task);*/
        }

        private async void UpdateUserTasks()
        {
            /*var tasksByUser =
                await _restService.GetTasksResponseAsync(Constants.TasksByUserAddress + "?user_id=" +
                                                         _loginResponse.Id);
            foreach (var task in tasksByUser)
                TasksByUser.Add(task);*/
        }

        private void ListView_tasks_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Application.Current.MainPage = new ViewTaskPage((TasksResponse)e.Item, _loginResponse);
        }

        private void Button_reloadListTasks_OnClicked(object sender, EventArgs e)
        {
            Tasks.Clear();
            UpdateTasks();
        }

        private void Button_updateUserTasks_OnClicked(object sender, EventArgs e)
        {
            TasksByUser.Clear();
            UpdateUserTasks();
        }
    }
}