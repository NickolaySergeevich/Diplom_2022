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

        private ObservableCollection<TasksResponse> _tasks;
        public ObservableCollection<TasksResponse> Tasks => _tasks ?? (_tasks = new ObservableCollection<TasksResponse>());

        public MainUserPage()
        {
            InitializeComponent();

            _restService = new RestService();

            listView_tasks.ItemsSource = Tasks;
        }

        protected override void OnAppearing()
        {
            UpdateTasks();
        }

        private async void UpdateTasks()
        {
            var tasks = await _restService.GetResponseAsync<List<TasksResponse>>(Constants.TasksAddress);
            foreach (var task in tasks)
                Tasks.Add(task);
        }

        private void ListView_tasks_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Application.Current.MainPage = new ViewTaskPage(Tasks[e.ItemIndex]);
        }

        private void Button_reloadListTasks_OnClicked(object sender, EventArgs e)
        {
            Tasks.Clear();
            UpdateTasks();
        }
    }
}