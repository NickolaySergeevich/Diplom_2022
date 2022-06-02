using System;
using System.Collections.Generic;
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

        private readonly UserInformationResponse _userInformation;

        private ObservableCollection<TasksResponse> _tasks;
        public ObservableCollection<TasksResponse> Tasks => _tasks ?? (_tasks = new ObservableCollection<TasksResponse>());
        private ObservableCollection<TasksByUserResponse> _tasksByUser;
        public ObservableCollection<TasksByUserResponse> TasksByUser => _tasksByUser ?? (_tasksByUser = new ObservableCollection<TasksByUserResponse>());

        public MainUserPage(UserInformationResponse userInformation)
        {
            InitializeComponent();

            _restService = new RestService();

            _userInformation = userInformation;

            listView_tasks.ItemsSource = Tasks;
            listView_tasksByUser.ItemsSource = TasksByUser;

            label_username.Text = userInformation.Username;
            label_name.Text = userInformation.Name;
            label_surname.Text = userInformation.Surname;
            label_patronymic.Text = userInformation.Patronymic;
            label_country.Text = userInformation.Country;
            label_city.Text = userInformation.City;
            label_educationalInstitution.Text = userInformation.EducationalInstitution;
            label_classNumber.Text = userInformation.ClassNumber.ToString();
            label_email.Text = userInformation.Email;
            label_phoneNumber.Text = userInformation.PhoneNumber;
        }

        protected override void OnAppearing()
        {
            UpdateTasks();
            UpdateTasksByUser();
        }

        private async void UpdateTasks()
        {
            var tasks = await _restService.GetResponseAsync<List<TasksResponse>>(Constants.TasksAddress);
            foreach (var task in tasks)
                Tasks.Add(task);
        }

        private async void UpdateTasksByUser()
        {
            var tasks = await _restService.GetResponseAsync<List<TasksByUserResponse>>(Constants.TasksByUserAddress +
                "?user_id=" + _userInformation.UserId);
            foreach (var task in tasks)
                TasksByUser.Add(task);
        }

        private void ListView_tasks_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Application.Current.MainPage = new ViewTaskPage(_userInformation, Tasks[e.ItemIndex]);
        }

        private void Button_reloadListTasks_OnClicked(object sender, EventArgs e)
        {
            Tasks.Clear();
            UpdateTasks();
        }

        private void Button_reloadListTasksByUser_OnClicked(object sender, EventArgs e)
        {
            TasksByUser.Clear();
            UpdateTasksByUser();
        }
    }
}