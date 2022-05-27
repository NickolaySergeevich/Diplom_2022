using System.Collections.ObjectModel;

using MobileApp.ApiJsonResponse;

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

        protected override async void OnAppearing()
        {
            var tasks = await _restService.GetTasksResponseAsync(Constants.TasksAddress);
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }
    }
}