using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

using MobileApp.ApiJsonResponse;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage
    {
        private readonly RestService _restService;

        private ObservableCollection<TasksResponse> _tasks;
        public ObservableCollection<TasksResponse> Tasks => _tasks ?? (_tasks = new ObservableCollection<TasksResponse>());

        public LoginPage()
        {
            InitializeComponent();

            _restService = new RestService();

            listView_tasks.ItemsSource = Tasks;
        }

        protected override async void OnAppearing()
        {
            var tasks = await _restService.GetResponseAsync<List<TasksResponse>>(Constants.TasksAddress);
            foreach (var task in tasks)
                Tasks.Add(task);
        }

        private async void Button_login_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entry_login.Text) || string.IsNullOrEmpty(entry_password.Text))
            {
                await DisplayAlert("Внимание", "Не заполнены поля для входа", "OK");
                return;
            }

            var login = entry_login.Text;
            var password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(entry_password.Text))).Replace("-", string.Empty);

            button_login.IsEnabled = false;

            var response = await _restService.GetResponseAsync<LoginResponse>(Constants.LoginAddress + "?username=" + login + "&password=" + password);
            switch (response.Status)
            {
                case Constants.ServerError:
                    await DisplayAlert("Ooops", "С сервером что-то не так. Обратитесь к системному администратору.", "OK");
                    break;
                case Constants.NoDataInDb:
                    await DisplayAlert("Вы не зарегистрированы!", "Не можем найти ваши данные на сервере. Пройдите регистрацию.", "OK");
                    break;
                default:
                    switch (response.RoleId)
                    {
                        case Constants.JustUser:
                            var responseTwo = await _restService.GetResponseAsync<UserInformationResponse>(
                                Constants.UserInformationAddress + "?user_id=" + response.Id + "&username=" + login +
                                "&password=" + password);

                            switch (responseTwo.Status)
                            {
                                case Constants.ServerError:
                                    await DisplayAlert("Ooops", "С сервером что-то не так. Обратитесь к системному администратору.", "OK");
                                    break;
                                case Constants.NoDataInDb:
                                    await DisplayAlert("Вы не зарегистрированы!", "Не можем найти ваши данные на сервере. Пройдите регистрацию.", "OK");
                                    break;
                                default:
                                    Application.Current.MainPage = new MainUserPage(responseTwo);
                                    break;
                            }

                            break;
                        case Constants.NastUser:
                            break;
                        case Constants.OrgUser:
                            break;
                        case Constants.PartnerUser:
                            break;
                        case Constants.ExpertUser:
                            break;
                    }

                    break;
            }

            button_login.IsEnabled = true;
        }

        private void Button_registration_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegistrationUserPage();
        }

        private void ListView_tasks_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Application.Current.MainPage = new ViewTasksWithoutSignUpPage(Tasks[e.ItemIndex]);
        }

        private void Button_reloadTaskList_OnClicked(object sender, EventArgs e)
        {
            Tasks.Clear();
            OnAppearing();
        }
    }
}